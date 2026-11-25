using PoTracking.Abstractions;
using POTracking.Domain;
using System.Collections.Concurrent;

namespace POTracking.Data.InMemory
{
    public class PurchaseOrderRepository : IPurchaseOrderRepository
    {
        private readonly ConcurrentDictionary<string, PurchaseOrder> _purchaseOrders = new(comparer:StringComparer.OrdinalIgnoreCase); // Use ExternalRef as the key (case-insensitive). Using ConcurrentDictionary for thread-safety. Dictionary is not thread safe.
        private int _nextPurchaseOrderNumber;

        public bool Create(PurchaseOrder purchaseOrder)
        {
            if (!_purchaseOrders.TryAdd(purchaseOrder.ExternalRef, purchaseOrder)) // Will fail/return false if the external ref already exists in the list of purchase orders.
                return false;

            purchaseOrder.Number = $"PO{Interlocked.Increment(ref _nextPurchaseOrderNumber):0000}"; // Make it thread safe, else we could get duplicated numbers.
            // When switching to a Database, this could be the auto incremented row id/PK.


            return true;
        }
    }
}