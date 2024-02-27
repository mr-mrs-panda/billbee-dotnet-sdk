using System.Collections.Specialized;
using Panda.NuGet.BillbeeClient.Endpoints.Interfaces;
using Panda.NuGet.BillbeeClient.Enums;
using Panda.NuGet.BillbeeClient.Models;
using Panda.NuGet.BillbeeClient.Utilities;

namespace Panda.NuGet.BillbeeClient.Endpoints
{
    /// <inheritdoc cref="IOrderEndPoint" />
    public class OrderEndPoint : IOrderEndPoint
    {
        private readonly IBillbeeRestClient _restClient;
        private readonly IRateLimiter _rateLimiter;

        public OrderEndPoint(IBillbeeRestClient restClient, IRateLimiter rateLimiter)
        {
            _restClient = restClient;
            _rateLimiter = rateLimiter;
        }

        public async Task<ApiResult<Order>> GetOrderAsync(string id, int articleTitleSource = 0)
        {
            await _rateLimiter.ThrottleAsync($"{nameof(GetOrderAsync)}-{id}", 6, 60);
            return await _restClient.GetAsync<ApiResult<Order>>($"/orders/{id}?articleTitleSource={articleTitleSource}");
        }
        
        public async Task<ApiResult<Order>> GetOrderAsync(long id, int articleTitleSource = 0)
        {
            await _rateLimiter.ThrottleAsync($"{nameof(GetOrderAsync)}-{id}", 6, 60);
            return await _restClient.GetAsync<ApiResult<Order>>($"/orders/{id}?articleTitleSource={articleTitleSource}");
        }

        public async Task<ApiResult<List<string>>> GetPatchableFieldsAsync()
        {
            return await _restClient.GetAsync<ApiResult<List<string>>>("/orders/PatchableFields");
        }

        public async Task<ApiResult<Order>> PatchOrderAsync(long id, Dictionary<string, object> fieldsToPatch)
        {
            return await _restClient.PatchAsync<ApiResult<Order>, Dictionary<string, object>>($"/orders/{id}", fieldsToPatch);
        }

        public async Task<ApiResult<Order>> GetOrderByExternalReferenceAsync(string id)
        {
            return await _restClient.GetAsync<ApiResult<Order>>($"/orders/findbyextref/{id}");
        }

        public async Task<ApiResult<Order>> GetOrderByExternalIdAndPartnerAsync(string partner, string? id)
        {
            return await _restClient.GetAsync<ApiResult<Order>>($"/orders/find/{id}/{partner}");
        }

        public async Task<ApiPagedResult<List<Order>>> GetOrderListAsync(DateTime? minOrderDate = null,
            DateTime? maxOrderDate = null,
            int page = 1,
            int pageSize = 50,
            List<long>? shopId = null,
            List<OrderStateEnum>? orderStateId = null,
            List<string>? tag = null,
            long? minimumBillBeeOrderId = null,
            DateTime? modifiedAtMin = null,
            DateTime? modifiedAtMax = null,
            bool excludeTags = false)
        {
            var parameters = new NameValueCollection();

            if (minOrderDate != null)
            {
                parameters.Add("minOrderDate", minOrderDate.Value.ToString("yyyy-MM-dd HH:mm"));
            }

            if (maxOrderDate != null)
            {
                parameters.Add("maxOrderDate", maxOrderDate.Value.ToString("yyyy-MM-dd HH:mm"));
            }

            if (modifiedAtMin != null)
            {
                parameters.Add("modifiedAtMin", modifiedAtMin.Value.ToString("yyyy-MM-dd HH:mm"));
            }

            if (modifiedAtMax != null)
            {
                parameters.Add("modifiedAtMax", modifiedAtMax.Value.ToString("yyyy-MM-dd HH:mm"));
            }

            if (minimumBillBeeOrderId != null)
            {
                parameters.Add("minimumBillBeeOrderId", minimumBillBeeOrderId.ToString());
            }

            if (shopId != null)
            {
                var i = 0;
                foreach (var id in shopId)
                {
                    parameters.Add($"shopId[{i++}]", id.ToString());
                }
            }

            if (tag != null)
            {
                var i = 0;
                foreach (var id in tag)
                {
                    parameters.Add($"tag[{i++}]", id);
                }
            }

            if (orderStateId != null)
            {
                var i = 0;
                foreach (var id in orderStateId)
                {
                    parameters.Add($"orderStateId[{i++}]", ((int)id).ToString());
                }
            }

            parameters.Add("page", page.ToString());
            parameters.Add("pageSize", pageSize.ToString());
            parameters.Add("excludeTags", excludeTags.ToString());

            return await _restClient.GetAsync<ApiPagedResult<List<Order>>>("/orders", parameters);
        }

        public async Task<ApiResult<List<InvoiceDetail>>> GetInvoiceListAsync(
            DateTime? minInvoiceDate = null,
            DateTime? maxInvoiceDate = null,
            int page = 1, int pageSize = 50,
            List<long>? shopId = null,
            List<int>? orderStateId = null,
            List<string>? tag = null,
            DateTime? minPayDate = null,
            DateTime? maxPayDate = null,
            bool includePositions = false,
            bool excludeTags = false)
        {
            await _rateLimiter.ThrottleAsync($"{nameof(GetInvoiceListAsync)}-{page}-{minInvoiceDate}", 1, 60);
            var parameters = new NameValueCollection();

            if (minInvoiceDate != null)
            {
                parameters.Add("minInvoiceDate", minInvoiceDate.Value.ToString("yyyy-MM-dd HH:mm"));
            }

            if (maxInvoiceDate != null)
            {
                parameters.Add("maxInvoiceDate", maxInvoiceDate.Value.ToString("yyyy-MM-dd HH:mm"));
            }

            if (minPayDate != null)
            {
                parameters.Add("minPayDate", minPayDate.Value.ToString("yyyy-MM-dd HH:mm"));
            }

            if (maxPayDate != null)
            {
                parameters.Add("maxPayDate", maxPayDate.Value.ToString("yyyy-MM-dd HH:mm"));
            }

            if (shopId != null)
            {
                var i = 0;
                foreach (var id in shopId)
                {
                    parameters.Add($"shopId[{i++}]", id.ToString());
                }
            }

            if (tag != null)
            {
                var i = 0;
                foreach (var id in tag)
                {
                    parameters.Add($"tag[{i++}]", id);
                }
            }

            if (orderStateId != null)
            {
                var i = 0;
                foreach (var id in orderStateId)
                {
                    parameters.Add($"orderStateId[{i++}]", id.ToString());
                }
            }


            parameters.Add("includePositions", includePositions.ToString());
            parameters.Add("page", page.ToString());
            parameters.Add("pageSize", pageSize.ToString());
            parameters.Add("excludeTags", excludeTags.ToString());

            return await _restClient.GetAsync<ApiResult<List<InvoiceDetail>>>("/orders/invoices", parameters);
        }

        public async Task<ApiResult<OrderResult>> PostNewOrderAsync(Order order, long shopId)
        {
            return await _restClient.PostAsync<ApiResult<OrderResult>, Order>($"/orders?shopId={shopId}", order);
        }
        
        public async Task<ApiResult<Order>> PostNewOrderAsync(Order order)
        {
            return await _restClient.PostAsync<ApiResult<Order>, Order>("/orders", order);
        }
        
        public async Task AddTagsAsync(List<string> tags, long orderId)
        {
            await _restClient.PostAsync($"/orders/{orderId}/tags", new TagsRequest { Tags = tags });
        }

        public async Task UpdateTagsAsync(List<string> tags, long orderId)
        {
            await _restClient.PutAsync($"/orders/{orderId}/tags", new TagsRequest { Tags = tags });
        }
        
        public async Task AddShipmentAsync(OrderShipment shipment)
        {
            await _restClient.PostAsync($"/orders/{shipment.OrderId}/shipment", shipment);
        }

        public async Task<ApiResult<DeliveryNote>> CreateDeliveryNoteAsync(long orderId, bool includePdf = false, long? sendToCloudId = null)
        {
            await _rateLimiter.ThrottleAsync($"{nameof(CreateDeliveryNoteAsync)}-{orderId}", 1, 60 * 5);
            
            var path = $"/orders/CreateDeliveryNote/{orderId}?includePdf={includePdf}";
            if (sendToCloudId.HasValue)
            {
                path += $"&sendToCloudId={sendToCloudId}";
            }
            
            return await _restClient.PostAsync<ApiResult<DeliveryNote>>(path);
        }

        public async Task<ApiResult<Invoice>> CreateInvoiceAsync(long orderId, bool includePdf = false, long? templateId = null, long? sendToCloudId = null)
        {
            await _rateLimiter.ThrottleAsync($"{nameof(CreateInvoiceAsync)}-{orderId}", 1, 60 * 5);
            
            var path = $"/orders/CreateInvoice/{orderId}?includePdf={includePdf}";
            if (sendToCloudId.HasValue)
            {
                path += $"&sendToCloudId={sendToCloudId}";
            }
            
            if (templateId.HasValue)
            {
                path += $"&templateId={templateId}";
            }

            return await _restClient.PostAsync<ApiResult<Invoice>>(path);
        }

        public async Task ChangeOrderStateAsync(long id, OrderStateEnum state)
        {
            await _restClient.PutAsync($"/orders/{id}/orderstate", new OrderStateRequest { NewStateId = (int)state });
        }

        public async Task SendMailForOrderAsync(long orderId, SendMessage message)
        {
            await _restClient.PostAsync($"/orders/{orderId}/send-message", message);
        }

        public async Task CreateEventAtOrderAsync(long orderId, string? eventName, uint delayInMinutes = 0)
        {
            var model = new TriggerEventContainer
            {
                DelayInMinutes = delayInMinutes,
                Name = eventName
            };

            await _restClient.PostAsync($"/orders/{orderId}/trigger-event", model);
        }

        public async Task<ApiResult<List<LayoutTemplate>>> GetLayoutsAsync()
        {
            return await _restClient.GetAsync<ApiResult<List<LayoutTemplate>>>("/layouts");
        }

        public async Task<ParsePlaceholdersResult> ParsePlaceholdersAsync(long orderId, ParsePlaceholdersQuery parsePlaceholdersQuery)
        {
            return await _restClient.PostAsync<ParsePlaceholdersResult, ParsePlaceholdersQuery>($"/orders/{orderId}/parse-placeholders", parsePlaceholdersQuery);
        }

        public async Task AddMessageAsync(long orderId, OrderMessage orderMessage)
        {
            await _restClient.PostAsync($"/orders/{orderId}/message", orderMessage);
        }
    }
}
