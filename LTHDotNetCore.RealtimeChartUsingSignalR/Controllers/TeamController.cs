using LTHDotNetCore.RealtimeChartUsingSignalR.Hubs;
using LTHDotNetCore.RealtimeChartUsingSignalR.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace LTHDotNetCore.RealtimeChartUsingSignalR.Controllers
{
    public class TeamController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly IHubContext<TeamHub> _hubContext;

        public TeamController(AppDbContext context, IHubContext<TeamHub> hubContext)
        {
            _appDbContext = context;
            _hubContext = hubContext;
        }

        [ActionName("Index")]
        public IActionResult TeamIndex()
        {
            return View("TeamIndex");
        }

        [ActionName("Create")]
        public IActionResult TeamCreate()
        {
            return View("TeamCreate");
        }

        [HttpPost]
        public async Task<IActionResult> Save(TeamDataModel model)
        {
            try
            {
                await _appDbContext.Teams.AddAsync(model);
                await _appDbContext.SaveChangesAsync();

                List<TeamDataModel> lst = await _appDbContext.Teams
                    .AsNoTracking()
                    .ToListAsync();

                var data = new
                {
                    Series = lst.Select(x => x.Score).ToList(),
                    Labels = lst.Select(x => x.TeamName).ToList()
                };
                string json = JsonConvert.SerializeObject(data);
                await _hubContext.Clients.All.SendAsync("ReceiveTeamClientEvent", json);

                return Redirect("/Team/Create");
            }
            catch
            {

                throw;
            }
        }
    }
}
