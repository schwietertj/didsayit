using System;
using System.Linq;
using System.Threading.Tasks;
using DidSayIt.Extensions;
using DidSayIt.Repository;
using DidSayItModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DidSayIt.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _ctx;
        private readonly IContentRepository _contentRepository;
        private readonly ILinkRepository _linkRepository;
        private readonly ISubdomainRepository _subdomainRepository;

        public AdminController(ApplicationDbContext ctx, IContentRepository contentRepository, ILinkRepository linkRepository, ISubdomainRepository subdomainRepository)
        {
            _ctx = ctx;
            _contentRepository = contentRepository;
            _linkRepository = linkRepository;
            _subdomainRepository = subdomainRepository;
        }

        public IActionResult Index()
        {
            return View(_subdomainRepository.GetAll().IncludeMultiple(x => x.Contents));
        }

        [Route("/Admin/Subdomain/{id}")]
        public async Task<IActionResult> Subdomain(long id)
        {
            var result = await _subdomainRepository.GetById(id);
            if (result is null)
                return RedirectToAction("Index");

            result.Contents = _contentRepository.GetAll().IncludeMultiple(x => x.Links).Where(x => x.SubdomainId == id).ToList();

            return View(result);
        }

        [Route("/Admin/Subdomain/{subdomainId}/Create")]
        public IActionResult CreateContent(long subdomainId)
        {
            return View(new DidSayItModels.App.Content { SubdomainId = subdomainId });
        }

        [Route("/Admin/Subdomain/{subdomainId}/Edit/{id}")]
        public async Task<IActionResult> EditContent(long subdomainId, long id)
        {
            var result = await _contentRepository.GetById(id);

            if (result is null)
                return RedirectToAction("Subdomain", new {id = subdomainId});

            return View(result);
        }

        [HttpPost]
        [Route("/Admin/Subdomain/{subdomainId}/Create")]
        public async Task<IActionResult> CreateContent(long subdomainId, DidSayItModels.App.Content model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                await _contentRepository.Save(model);
                return RedirectToAction("Subdomain", new { id = subdomainId });
            }
            catch (Exception e)
            {
                ViewData["errors"] = e.Message ?? "";
                return View(model);
            }
        }

        [HttpPost]
        [Route("/Admin/Subdomain/{subdomainId}/Edit/{id}")]
        public async Task<IActionResult> EditContent(long subdomainId, long id, DidSayItModels.App.Content model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                await _contentRepository.Save(model);
                return RedirectToAction("Subdomain", new { id = subdomainId });
            }
            catch (Exception e)
            {
                ViewData["errors"] = e.Message ?? "";
                return View(model);
            }

        }

        #region ajaxGets
        [HttpGet]
        public JsonResult GetAllSubDomains()
        {
            return Json(_subdomainRepository.GetAll());
        }

        [HttpGet]
        public async Task<JsonResult> GetSubDomain(long id)
        {
            return Json(await _subdomainRepository.GetById(id));
        }

        [HttpGet]
        public JsonResult GetAllContent(long subdomainId)
        {
            return Json(_contentRepository.GetAll().Where(x => x.SubdomainId == subdomainId));
        }

        [HttpGet]
        public async Task<JsonResult> GetContent(long subdomainId, long id)
        {
            var result = await _contentRepository.GetById(id) ?? new DidSayItModels.App.Content();
            return Json(result.SubdomainId != subdomainId ? null : result);
        }

        #endregion

        #region ajaxPosts
        public IActionResult CreateSubdomain(string name)
        {
            if (_subdomainRepository.GetAll().Any(x => x.Name == name.ToLower().Trim()))
                return BadRequest($"The subdomain, {name}, already exists.");

            try
            {
                _subdomainRepository.Save(new DidSayItModels.App.Subdomain {Name = name.ToLower().Trim()});
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }
        #endregion
    }
}