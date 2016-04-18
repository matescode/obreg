using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

using System.Data.SQLite;

namespace ObReg.Core
{
    internal class OrderItemDataAccessLayer : IOrderItemDataAccessLayer
    {
        private const string DatabaseFile = "ObregDatabase.db";

        private static OrderItemDataAccessLayer _instance = null;

        private readonly List<OrderItem> _items;

        private DataTable _dataTable;

        private SQLiteConnection _connection;
        private SQLiteDataAdapter _adapter;

        private bool _loaded;

        private OrderItemDataAccessLayer()
        {
            _loaded = false;
            _items = new List<OrderItem>();
        }

        private void OnAccess()
        {
            if (!_loaded)
            {
                Init();
                _loaded = true;
            }
        }

        private void Init()
        {
            if (!File.Exists(DatabaseFile))
            {
                InstallDatabase();
            }

            _connection = new SQLiteConnection(@"Data Source=" + DatabaseFile);
            _connection.Open();

            SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM orderItems", _connection);
            _adapter = new SQLiteDataAdapter(cmd);
            _adapter.AcceptChangesDuringUpdate = true;
            _adapter.AcceptChangesDuringFill = true;

            cmd = new SQLiteCommand(
                "INSERT INTO orderItems (id, code, description, count, finalCount, receiveDate, estimatedDate, terminationDate, status) " +
                "VALUES(@Id, @Code, @Description, @Count, @FinalCount, @ReceiveDate, @EstimatedDate, @TerminationDate, @Status)", _connection
            );

            cmd.Parameters.Add(new SQLiteParameter("@Id", DbType.Int32, "id"));
            cmd.Parameters.Add("@Code", DbType.String, 100, "code");
            cmd.Parameters.Add("@Description", DbType.String, 100, "description");
            cmd.Parameters.Add(new SQLiteParameter("@Count", DbType.Int32, "count"));
            cmd.Parameters.Add(new SQLiteParameter("@FinalCount", DbType.Int32, "finalCount"));
            cmd.Parameters.Add(new SQLiteParameter("@ReceiveDate", DbType.String, "receiveDate"));
            cmd.Parameters.Add(new SQLiteParameter("@EstimatedDate", DbType.String, "estimatedDate"));
            cmd.Parameters.Add(new SQLiteParameter("@TerminationDate", DbType.String, "terminationDate"));
            cmd.Parameters.Add(new SQLiteParameter("@Status", DbType.Int32, "status"));

            _adapter.InsertCommand = cmd;

            cmd = new SQLiteCommand(
                "DELETE FROM orderItems WHERE id = @Id", _connection
            );

            cmd.Parameters.Add(new SQLiteParameter("@Id", DbType.Int32, "id"));

            _adapter.DeleteCommand = cmd;

            cmd = new SQLiteCommand(
                "UPDATE orderItems SET code = @Code, description = @Description, count = @Count, finalCount = @FinalCount, receiveDate = @ReceiveDate, estimatedDate = @EstimatedDate, terminationDate = @TerminationDate, status = @Status WHERE id = @Id", _connection
            );

            cmd.Parameters.Add("@Code", DbType.String, 100, "code");
            cmd.Parameters.Add("@Description", DbType.String, 100, "description");
            cmd.Parameters.Add(new SQLiteParameter("@Count", DbType.Int32, "count"));
            cmd.Parameters.Add(new SQLiteParameter("@FinalCount", DbType.Int32, "finalCount"));
            cmd.Parameters.Add(new SQLiteParameter("@ReceiveDate", DbType.String, "receiveDate"));
            cmd.Parameters.Add(new SQLiteParameter("@EstimatedDate", DbType.String, "estimatedDate"));
            cmd.Parameters.Add(new SQLiteParameter("@TerminationDate", DbType.String, "terminationDate"));
            cmd.Parameters.Add(new SQLiteParameter("@Status", DbType.Int32, "status"));
            cmd.Parameters.Add(new SQLiteParameter("@Id", DbType.Int32, "id"));

            _adapter.UpdateCommand = cmd;

            _dataTable = new DataTable();
            _adapter.Fill(_dataTable);

            _items.Clear();
            foreach (DataRow row in _dataTable.Rows)
            {
                _items.Add(new OrderItem(row));
            }
        }

        private void InstallDatabase()
        {
            using (SQLiteConnection conn = new SQLiteConnection(@"Data Source=" + DatabaseFile))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(
                    "CREATE TABLE orderItems (" +
                    "id integer primary key autoincrement, " +
                    "code text not null, " +
                    "description text not null, " +
                    "count integer not null, " +
                    "finalCount integer not null, " +
                    "receiveDate text not null, " +
                    "estimatedDate text, " +
                    "terminationDate text, " +
                    "status integer not null " +
                    ");"
                    , conn
                );
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        public static IOrderItemDataAccessLayer Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new OrderItemDataAccessLayer();
                }
                return _instance;
            }
        }

        #region IOrderItemDataAccessLayer Members

        public IEnumerable<OrderItem> GetData(OrderItemType itemType, SearchParameter param = null)
        {
            OnAccess();
            if (itemType == OrderItemType.Unresolved)
            {
                return _items.Where(item => item.Status == 0);
            }
            else if (itemType == OrderItemType.Resolved)
            {
                return _items.Where(item => item.Status == 1 && item.TerminationDate.HasValue && item.TerminationDate.Value.Year == DateTime.Now.Year);
            }
            else if (itemType == OrderItemType.Results && param != null)
            {
                return _items.Where(item => item.Status == (int)param.ItemType && (string.IsNullOrEmpty(param.Code) || item.Code == param.Code) && (param.Year == -1 || (item.TerminationDate.HasValue && item.TerminationDate.Value.Year == param.Year)) && (param.Month == -1 || (item.TerminationDate.HasValue && item.TerminationDate.Value.Month == param.Month)));
            }
            else if (itemType == OrderItemType.History)
            {
                return _items.Where(item => item.Status == 1 && item.TerminationDate.HasValue && item.TerminationDate.Value.Year <= DateTime.Now.Year && (param == null || param.Code == item.Code));
            }
            return null;
        }

        public IEnumerable<HistoryDateHierarchy> GetHistorySignatures()
        {
            return GetData(OrderItemType.History).GroupBy(item => item.TerminationDate.Value.Year).Select(item => new HistoryDateHierarchy(item.Key, item.AsEnumerable().Select(months => months.TerminationDate.Value.Month).Distinct()));
        }

        public IEnumerable<OrderItem> GetHistoryData(int year, int month = -1)
        {
            return _items.Where(item => item.Status == 1 && item.TerminationDate.HasValue && item.TerminationDate.Value.Year == year && (item.TerminationDate.Value.Month == month || month == -1));
        }

        public void UpdateOrderItem(OrderItem orderItem)
        {
            if (orderItem.IsNew)
            {
                _items.Add(orderItem);
                orderItem.IsNew = false;
            }
            orderItem.IsUpdated = true;
        }

        public void DeleteOrderItem(OrderItem orderItem)
        {
            if (!orderItem.IsNewItem)
            {
                FindRow(orderItem.Id).Delete();
            }
            _items.Remove(orderItem);
        }

        public void ResolveItem(OrderItem orderItem)
        {
            orderItem.Status = 1;
        }

        public void SaveChanges()
        {
            foreach (OrderItem item in _items)
            {
                if (item.IsNewItem)
                {
                    DataRow newRow = _dataTable.NewRow();
                    item.UpdateData(newRow);
                    _dataTable.Rows.Add(newRow);
                }
                else
                {
                    if (item.IsUpdated)
                    {
                        DataRow row = FindRow(item.Id);
                        item.UpdateData(row);
                    }
                }
            }
            _adapter.Update(_dataTable);
        }

        public void Destroy()
        {
            _connection.Close();
            _connection.Dispose();
        }

        public IEnumerable<KeyValuePair<string, string>> GetAutoCompleteValues()
        {
            List<KeyValuePair<string, string>> values = new List<KeyValuePair<string, string>>();

            List<string> codes = new List<string>();
            codes.AddRange(_items.Select(item => item.Code).Distinct().OrderBy(item => item));

            foreach (string code in codes)
            {
                string text = _items.First(item => item.Code == code).Text;
                values.Add(new KeyValuePair<string, string>(code, text));
            }

            return values;
        }

        public void Preload()
        {
            OnAccess();
        }

        #endregion

        #region Internals and Helpers

        private DataRow FindRow(long id)
        {
            DataRow[] rows = _dataTable.Select(String.Format("Id = {0}", id));
            if (rows.Length != 1)
            {
                throw new IndexOutOfRangeException(String.Format("Fatal error: too much rows selected for Id = {0}!", id));
            }
            return rows[0];
        }

        #endregion
    }
}
