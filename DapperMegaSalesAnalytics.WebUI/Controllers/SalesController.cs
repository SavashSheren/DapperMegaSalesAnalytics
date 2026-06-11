using DapperMegaSalesAnalytics.BusinessLayer.Abstract;
using DapperMegaSalesAnalytics.DtoLayer.Dtos.SalesTransactionDtos;
using DapperMegaSalesAnalytics.WebUI.Models;
using Microsoft.AspNetCore.Mvc;
namespace DapperMegaSalesAnalytics.WebUI.Controllers
{
    public class SalesController : Controller
    {
        private readonly ISalesTransactionService _salesTransactionService;

        public SalesController(ISalesTransactionService salesTransactionService)
        {
            _salesTransactionService = salesTransactionService;
        }

        public async Task<IActionResult> Index(
    int page = 1,
    int pageSize = 20,
    int? searchId = null,
    string? searchTerm = null,
    string? city = null,
    string? category = null,
    string? status = null,
    string? paymentMethod = null,
    string? salesChannel = null,
    DateTime? startDate = null,
    DateTime? endDate = null,
    decimal? minPrice = null,
    decimal? maxPrice = null,
    int? editId = null)
        {
            if (page < 1)
            {
                page = 1;
            }

            if (pageSize != 10 && pageSize != 20 && pageSize != 50 && pageSize != 100)
            {
                pageSize = 20;
            }

            var filter = new FilterSalesTransactionDto
            {
                Page = page,
                PageSize = pageSize,
                SearchId = searchId,
                SearchTerm = searchTerm,
                City = city,
                Category = category,
                Status = status,
                PaymentMethod = paymentMethod,
                SalesChannel = salesChannel,
                StartDate = startDate,
                EndDate = endDate,
                MinPrice = minPrice,
                MaxPrice = maxPrice
            };

            var totalCount = await _salesTransactionService.TGetFilteredSalesTransactionCountAsync(filter);
            var totalPages = totalCount == 0 ? 0 : (int)Math.Ceiling(totalCount / (double)pageSize);

            if (totalPages > 0 && page > totalPages)
            {
                page = totalPages;
                filter.Page = page;
            }

            var transactions = await _salesTransactionService.TGetFilteredSalesTransactionsAsync(filter);
            var filterOptions = await _salesTransactionService.TGetSalesFilterOptionsAsync();

            var model = new SalesTransactionListViewModel
            {
                Transactions = transactions,
                Filter = filter,
                FilterOptions = filterOptions,
                CurrentPage = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = totalPages,
                SearchId = searchId
            };

            if (searchId.HasValue && totalCount == 0)
            {
                model.Message = $"No transaction found with ID {searchId.Value}.";
            }

            if (editId.HasValue)
            {
                var transactionForEdit = await _salesTransactionService.TGetSalesTransactionByIdAsync(editId.Value);

                if (transactionForEdit is not null)
                {
                    model.EditTransaction = new UpdateSalesTransactionDto
                    {
                        SalesTransactionId = transactionForEdit.SalesTransactionId,
                        CustomerFullName = transactionForEdit.CustomerFullName,
                        CustomerEmail = transactionForEdit.CustomerEmail,
                        City = transactionForEdit.City,
                        ProductName = transactionForEdit.ProductName,
                        ProductCategory = transactionForEdit.ProductCategory,
                        Quantity = transactionForEdit.Quantity,
                        UnitPrice = transactionForEdit.UnitPrice,
                        TotalPrice = transactionForEdit.TotalPrice,
                        OrderStatus = transactionForEdit.OrderStatus,
                        PaymentMethod = transactionForEdit.PaymentMethod,
                        SalesChannel = transactionForEdit.SalesChannel,
                        DeliveryDay = transactionForEdit.DeliveryDay,
                        CustomerAge = transactionForEdit.CustomerAge
                    };
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateSalesTransactionDto updateSalesTransactionDto)
        {
            updateSalesTransactionDto.TotalPrice = updateSalesTransactionDto.Quantity * updateSalesTransactionDto.UnitPrice;

            await _salesTransactionService.TUpdateSalesTransactionAsync(updateSalesTransactionDto);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _salesTransactionService.TDeleteSalesTransactionAsync(id);

            return RedirectToAction(nameof(Index));
        }
    }
}