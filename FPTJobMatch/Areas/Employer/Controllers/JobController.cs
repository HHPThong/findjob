﻿using FPTJobMatch.Models;
using FPTJobMatch.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FPTJobMatch.Areas.Employer.Controllers
{
    [Area("Employer")]
    [Authorize(Roles = "Employer")]
    public class JobController : Controller
    {
        private readonly IJobRepository _jobRepository;
        private readonly ITimeWorkRepository _workRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        private JobController(IJobRepository jobRepository, ITimeWorkRepository workRepository, IWebHostEnvironment webHostEnvironment)
        {
            _jobRepository = jobRepository;
            _workRepository = workRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            List<Job> myList = _jobRepository.GetAll().ToList();
            return View(myList);
        }
        [Authorize(Roles = "Employer")]
        
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Job job)
        {
            if (job.Name.Equals(job.Description))
            {
                ModelState.AddModelError("Description", "Name can not be the same as description");
            }
            if (ModelState.IsValid)
            {
                _jobRepository.Add(job);
                _jobRepository.Save();
                TempData["success"] = "Job Created successfully";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Edit(int? jobId)
        {
            if (jobId == null || jobId == 0)
            {
                return NotFound();
            }
            Job? job = _jobRepository.Get(j => j.ID == jobId);
            if (job == null)
            {
                return NotFound();
            }
            return View(job);
        }

        [HttpPost]
        public IActionResult Edit(Job job)
        {
            if (job.Name.Equals(job.Description))
            {
                ModelState.AddModelError("Description", "Name can not be the same as description");
            }
            if (ModelState.IsValid)
            {
                _jobRepository.Update(job);
                _jobRepository.Save();
                TempData["success"] = "Job updated successfully";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Delete(int? JobID)
        {
            if (JobID == null || JobID == 0)
            {
                return NotFound();
            }
            Job? job = _jobRepository.Get(j => j.ID == JobID);
            if (job == null)
            {
                return NotFound();
            }
            return View(job);
        }
        [HttpPost]
        public IActionResult Delete(Job job)
        {
            _jobRepository.Delete(job);
            _jobRepository.Save();
            TempData["success"] = "Job Deleted successfully";
            return RedirectToAction("Index");
        }
    }
}
