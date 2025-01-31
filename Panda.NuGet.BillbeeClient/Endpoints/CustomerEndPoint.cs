using System.Collections.Specialized;
using Panda.NuGet.BillbeeClient.Endpoints.Interfaces;
using Panda.NuGet.BillbeeClient.Exceptions;
using Panda.NuGet.BillbeeClient.Models;

namespace Panda.NuGet.BillbeeClient.Endpoints
{
    /// <inheritdoc cref="ICustomerEndPoint" />
    public class CustomerEndPoint: ICustomerEndPoint
    {
        private readonly IBillbeeRestClient _restClient;

        public CustomerEndPoint(IBillbeeRestClient restClient)
        {
            _restClient = restClient;
        }

        public async Task<ApiPagedResult<List<Customer>>> GetCustomerListAsync(int page, int pageSize)
        {
            var parameters = new NameValueCollection
            {
                {"page", page.ToString()},
                {"pageSize", pageSize.ToString()}
            };

            return await _restClient.GetAsync<ApiPagedResult<List<Customer>>>("/customers", parameters);
        }

        public async Task<ApiResult<Customer>> AddCustomerAsync(CustomerForCreation customer)
        {
            return await _restClient.PostAsync<ApiResult<Customer>, CustomerForCreation>("/customers", customer);
        }

        public async Task<ApiResult<Customer>> GetCustomerAsync(long id)
        {
            return await _restClient.GetAsync<ApiResult<Customer>>($"/customers/{id}");
        }

        public async Task<ApiResult<Customer>> UpdateCustomerAsync(Customer customer)
        {
            if (customer.Id == null)
            {
                throw new InvalidValueException("Id must not be null.");
            }
            return await _restClient.PutAsync<ApiResult<Customer>, Customer>($"/customers/{customer.Id}", customer);

        }

        public async Task<ApiPagedResult<List<Order>>> GetOrdersForCustomerAsync(long id, int page, int pageSize)
        {
            var parameters = new NameValueCollection
            {
                {"page", page.ToString()},
                {"pageSize", pageSize.ToString()}
            };
            return await _restClient.GetAsync<ApiPagedResult<List<Order>>>($"/customers/{id}/orders", parameters);
        }

        public async Task<ApiPagedResult<List<CustomerAddress>>> GetAddressesForCustomerAsync(long id, int page, int pageSize)
        {
            var parameters = new NameValueCollection
            {
                {"page", page.ToString()},
                {"pageSize", pageSize.ToString()}
            };
            return await _restClient.GetAsync<ApiPagedResult<List<CustomerAddress>>>($"/customers/{id}/addresses", parameters);
        }

        public async Task<ApiResult<CustomerAddress>> AddAddressToCustomerAsync(CustomerAddress customerAddress)
        {
            return await _restClient.PostAsync<ApiResult<CustomerAddress>, CustomerAddress>($"/customers/{customerAddress.CustomerId}/addresses", customerAddress);
        }

        public async Task<ApiResult<CustomerAddress>> GetCustomerAddressAsync(long customerAddressId)
        {
            return await _restClient.GetAsync<ApiResult<CustomerAddress>>($"/customers/addresses/{customerAddressId}");
        }

        public async Task<ApiResult<CustomerAddress>> UpdateCustomerAddressAsync(CustomerAddress customerAddress)
        {
            return await _restClient.PutAsync<ApiResult<CustomerAddress>, CustomerAddress>($"/customers/addresses/{customerAddress.Id}", customerAddress);
        }
        
        [Obsolete("Please us CustomerAddressesEndPoint.UpdateCustomerAddressAsync instead. The PATCH endpoint is not recommended since of frequent timeouts.")]
        public async Task<ApiResult<CustomerAddress>> PatchCustomerAddressAsync(long customerAddressId, Dictionary<string, string> fieldsToPatch)
        {
            return await _restClient.PatchAsync<ApiResult<CustomerAddress>, Dictionary<string, string>>($"/customers/addresses/{customerAddressId}", fieldsToPatch);
        }
    }
}
