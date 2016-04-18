using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ObReg.Core
{
    public interface IOrderItemDataAccessLayer
    {
        IEnumerable<OrderItem> GetData(OrderItemType itemType, SearchParameter param = null);

        IEnumerable<HistoryDateHierarchy> GetHistorySignatures();

        IEnumerable<OrderItem> GetHistoryData(int year, int month = -1);

        void UpdateOrderItem(OrderItem orderItem);

        void DeleteOrderItem(OrderItem orderItem);

        void ResolveItem(OrderItem orderItem);

        void SaveChanges();

        void Destroy();

        IEnumerable<KeyValuePair<string, string>> GetAutoCompleteValues();

        void Preload();
    }
}