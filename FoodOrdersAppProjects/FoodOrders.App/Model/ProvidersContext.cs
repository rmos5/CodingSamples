using FoodOrders.Model.Data;
using System;

namespace FoodOrders.Model
{
    public class ProvidersContext : FoodOrdersDataContextBase<Provider, ProviderContext>
    {
        class ProviderProductsContext : FoodOrdersDataContextBase<Product, ProductContext>
        {
            public ProviderContext ProviderContext { get; }

            public ProviderProductsContext(ProviderContext providerContext)
            {
                ProviderContext = providerContext ?? throw new ArgumentNullException(nameof(providerContext));
            }
        }
    }
}
