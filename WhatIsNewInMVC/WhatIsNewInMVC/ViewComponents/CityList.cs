using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WhatIsNewInMVC.Entities;

namespace WhatIsNewInMVC.ViewComponents;

public class CityList: ViewComponent
{
    private readonly PeopleHubDbContext db;

	public CityList(PeopleHubDbContext db)	
	{
		this.db = db;
	}

	public async Task<IViewComponentResult> InvokeAsync(string cityStartsWith)
	{
		var people = await db.People.Where(p => p.Address.City.StartsWith(cityStartsWith)).ToListAsync();
		return View(people);
	}
}
