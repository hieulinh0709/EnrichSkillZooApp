using BookStoreManagement.Core.Constants;
using BookStoreManagement.DataAccess.Repository.IRepository;
using BookStoreManagement.Models;
using BookStoreManagement.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreManagement.Web.Areas.Admin.Controllers;
[Area(ROLES.Admin)]
[Authorize(Roles = StatusData.Role_Admin)]
public class CoverTypeController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public CoverTypeController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public IActionResult Index()
    {
        IEnumerable<CoverType> objCoverTypeList = _unitOfWork.CoverTypeRepo.GetAll();
        return View(objCoverTypeList);
    }

    //GET
    public IActionResult Create()
    {
        return View();
    }

    //POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(CoverType obj)
    {
        if (ModelState.IsValid)
        {
            _unitOfWork.CoverTypeRepo.Add(obj);
            _unitOfWork.Save();
            TempData[STATUS.Success] = "CoverType created successfully";
            return RedirectToAction(ActionNameConsts.Index);
        }
        return View(obj);
    }

    //GET
    public IActionResult Edit(int? id)
    {
        if (id == null || id == 0) return NotFound();
        var coverTypeFromDbFirst = _unitOfWork.CoverTypeRepo.GetFirstOrDefault(u => u.Id == id);

        if (coverTypeFromDbFirst == null) return NotFound();

        return View(coverTypeFromDbFirst);
    }

    //POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(CoverType obj)
    {

        if (ModelState.IsValid)
        {
            _unitOfWork.CoverTypeRepo.Update(obj);
            _unitOfWork.Save();
            TempData[STATUS.Success] = "CoverType updated successfully";
            return RedirectToAction(ActionNameConsts.Index);
        }
        return View(obj);
    }

    public IActionResult Delete(int? id)
    {
        if (id == null || id == 0) return NotFound();
        var coverTypeFromDbFirst = _unitOfWork.CoverTypeRepo.GetFirstOrDefault(u => u.Id == id);

        if (coverTypeFromDbFirst == null) return NotFound();

        return View(coverTypeFromDbFirst);
    }

    //POST
    [HttpPost, ActionName(ActionNameConsts.Delete)]
    [ValidateAntiForgeryToken]
    public IActionResult DeletePOST(int? id)
    {
        var obj = _unitOfWork.CoverTypeRepo.GetFirstOrDefault(u => u.Id == id);
        if (obj == null) return NotFound();

        _unitOfWork.CoverTypeRepo.Remove(obj);
        _unitOfWork.Save();
        TempData[STATUS.Success] = "CoverType deleted successfully";
        return RedirectToAction(ActionNameConsts.Index);

    }
}
