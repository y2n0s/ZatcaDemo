using Application.Contracts.IServices;
using Application.Dtos.Requests;
using Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Presentation.Controllers
{
    public class ZatcaController : Controller
    {
        private readonly ICertificateCreationService _creationService;

        public ZatcaController(ICertificateCreationService creationService)
        {
            _creationService = creationService;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> AddSupplier()
        {
            var result = await _creationService.GetAllSellerIdentitiesAsync();
          
            var dropdownList = new SelectList(result.Data, "Key", "Description","CRN");

            ViewData["SellerIdentities"] = dropdownList;

            return View();
        }
        public async Task <IActionResult> CreateCertificate()
        {
            var result = await _creationService.GetAllInvoiceTypesAsync();

            var dropdownList = new SelectList(result.Data, "Key", "Description", "1100");

            ViewData["InvoiceTypes"] = dropdownList;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateCertificate(ZatcaCsrCreationRequestDto model)
        {
            if(!ModelState.IsValid) 

             return BadRequest();
            try
            {
                var result = await _creationService.CreateCertificateAsync(model);
                if (result.Success)
                {
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
           
            return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> AddSupplier(ZatcaSupplierCreationRequestDto model)
        {
            if (!ModelState.IsValid)

                return BadRequest();
            try
            {
                var result = await _creationService.AddSupplierAsync(model);
                if (result.Data != null)
                {
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            } 
            return BadRequest();
        }
    }
}
