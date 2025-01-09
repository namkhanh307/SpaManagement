using Core.Infrastructures;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Repos.ViewModels;
using Repos.ViewModels.TransactionVM;
using Repos.ViewModels.UserScheduleVM;
using Services.IServices;
using System.Transactions;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly IBaseService<PostTransactionVM, GetTransactionVM, GetTransactionVM, Transaction> _baseService;
        public TransactionController(IBaseService<PostTransactionVM, GetTransactionVM, GetTransactionVM, Transaction> baseService)
        {
            _baseService = baseService;
        }

        [HttpGet("get")]
        public async Task<IActionResult> GetTransaction(int pageNumber = 1, int pageSize = 10)
        {
            PagingVM<GetTransactionVM> result = await _baseService.GetAsync(null, null, null, pageNumber, pageSize);
            return Ok(new BaseResponseModel<PagingVM<GetTransactionVM>>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: result));
        }

        [HttpPost("post")]
        public async Task<IActionResult> PostTransaction(PostTransactionVM model)
        {
            await _baseService.PostAsync(model, null);
            return Ok(new BaseResponseModel<string>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: "Thêm transaction thành công"));
        }
    }
}
