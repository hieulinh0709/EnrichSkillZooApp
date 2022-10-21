using BookStoreManagement.Core.Constants;
using BookStoreManagement.DataAccess.Repository.IRepository;
using BookStoreManagement.Models;
using BookStoreManagement.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreManagement.Web.Areas.Admin.Controllers;
[Area(ROLES.Admin)]
[Authorize(Roles = StatusData.Role_Admin)] // Mọi action đều kiểm tra user đăng nhập là Admin mới thực hiện
public class CategoryController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public CategoryController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public IActionResult Index()
    {
        IEnumerable<Category> objCategoryList = _unitOfWork.CategoryRepo.GetAll();
        return View(objCategoryList);
    }

    //GET
    public IActionResult Create()
    {
        return View(ViewNameConsts.Create);
    }

    //POST
    [HttpPost]
    [ValidateAntiForgeryToken] // chặn Cross-site Request Forgery (CSRF) attacks
    public IActionResult Create(Category obj)
    {
        if (obj.Name == obj.DisplayOrder.ToString())
            ModelState.AddModelError("name", "The DisplayOrder cannot exactly match the Name.");

        if (ModelState.IsValid)
        {
            _unitOfWork.CategoryRepo.Add(obj);
            _unitOfWork.Save();
            TempData[STATUS.Success] = MSG.MsgCode7;
            return RedirectToAction(ActionNameConsts.Index);
        }
        return View(ViewNameConsts.Create, obj);
    }

    //GET
    public IActionResult Edit(int? id)
    {
        if (id == null || id == 0)
            return NotFound();

        var categoryFromDbFirst = _unitOfWork.CategoryRepo.GetFirstOrDefault(u => u.Id == id);

        var c = _unitOfWork.CategoryRepo.GetAll(null, null);
        if (categoryFromDbFirst == null)
            return NotFound();

        return View(ViewNameConsts.Edit, categoryFromDbFirst);
    }

    //POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Category obj)
    {
        if (obj.Name == obj.DisplayOrder.ToString())
            ModelState.AddModelError("name", "The DisplayOrder cannot exactly match the Name.");

        if (ModelState.IsValid)
        {
            _unitOfWork.CategoryRepo.Update(obj);
            _unitOfWork.Save();
            TempData[STATUS.Success] = MSG.MsgCode6;
            return RedirectToAction(ActionNameConsts.Index);
        }
        return View(ViewNameConsts.Edit, obj);
    }

    public IActionResult Delete(int? id)
    {
        if (id == null || id == 0)
            return NotFound();

        var categoryFromDbFirst = _unitOfWork.CategoryRepo.GetFirstOrDefault(u => u.Id == id);

        if (categoryFromDbFirst == null)
            return NotFound();

        return View(categoryFromDbFirst);
    }

    //POST
    [HttpPost, ActionName(ActionNameConsts.Delete)]
    [ValidateAntiForgeryToken]
    public IActionResult DeletePOST(int? id)
    {
        var obj = _unitOfWork.CategoryRepo.GetFirstOrDefault(u => u.Id == id);
        if (obj == null)
            return NotFound();

        _unitOfWork.CategoryRepo.Remove(obj);
        _unitOfWork.Save();
        TempData[STATUS.Success] = MSG.MsgCode5;
        return RedirectToAction(ActionNameConsts.Index);

    }
}
