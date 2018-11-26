using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DidSayIt.Repository;
using DidSayItModels;
using Microsoft.AspNetCore.Mvc;

namespace DidSayIt.Controllers
{
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
            return View();
        }


        
    }
}