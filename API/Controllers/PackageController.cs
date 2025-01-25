using Core.Infrastructures;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repos.ViewModels.ScheduleVM;
using Repos.ViewModels;
using Services.IServices;
using Services.Services;
using Repos.ViewModels.PackageVM;
using System.Linq.Expressions;
using Repos.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackageController(IPSService pSService) : ControllerBase
    {
        private readonly IPSService _pSService = pSService;

        [HttpGet("get")]
        public async Task<IActionResult> GetPackages(int pageNumber = 1, int pageSize = 10)
        {
            PagingVM<GetPackagesVM> result = await _pSService.GetPackages(pageNumber, pageSize);
            return Ok(new BaseResponseModel<PagingVM<GetPackagesVM>>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: result));
        }

        [HttpPost("post")]
        public async Task<IActionResult> PostPackage(PostPackageVM model)
        {
            await _pSService.PostPackage(model);
            return Ok(new BaseResponseModel<string>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: "Thêm package thành công"));
        }

        [HttpPut]
        public async Task<IActionResult> PutPackage(string packageId, PutPackageVM model)
        {
            await _pSService.PutPackage(packageId, model);
            return Ok(new BaseResponseModel<string>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: "Sửa package thành công"));
        }

        [HttpDelete]
        public async Task<IActionResult> DeletePackage(string packageId)
        {
            await _pSService.DeletePackage(packageId);
            return Ok(new BaseResponseModel<string>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: "Xóa package thành công"));
        }
    }
}
