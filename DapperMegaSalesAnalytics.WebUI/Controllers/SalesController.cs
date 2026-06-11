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

        public async Task<IActionResult> Index(int page = 1, int pageSize = 20, int? searchId = null, int? editId = null)
        {
            if (page < 1)
            {
                page = 1;
            }

            if (pageSize != 10 && pageSize != 20 && pageSize != 50 && pageSize != 100)
            {
                pageSize = 20;
            }

            var model = new SalesTransactionListViewModel
            {
                CurrentPage = page,
                PageSize = pageSize,
                SearchId = searchId
            };

            if (searchId.HasValue)
            {
                var transaction = await _salesTransactionService.TGetSalesTransactionByIdAsync(searchId.Value);

                if (transaction is not null)
                {
                    model.Transactions = new List<ResultSalesTransactionDto> { transaction };
                    model.TotalCount = 1;
                    model.TotalPages = 1;
                }
                else
                {
                    model.Transactions = new List<ResultSalesTransactionDto>();
                    model.TotalCount = 0;
                    model.TotalPages = 0;
                    model.Message = $"No transaction found with ID {searchId.Value}.";
                }
            }
            else
            {
                var totalCount = await _salesTransactionService.TGetTotalSalesTransactionCountAsync();
                var transactions = await _salesTransactionService.TGetPagedSalesTransactionsAsync(page, pageSize);

                model.Transactions = transactions;
                model.TotalCount = totalCount;
                model.TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
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