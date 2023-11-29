using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System.Globalization;

namespace oncall_api.Controllers
{
    [ApiController]
    public class OncallController : ControllerBase
    {
        [HttpGet]
        [Route("api/oncall/{year}")]
        public ActionResult GetAllYear(int year)
        {
            var db = new MongoClient(Configuration.mongoServer);
            var database = db.GetDatabase(Configuration.mongoDatabase);
            var collection = database.GetCollection<Oncall>(Configuration.oncallCollection);

            var results = collection.AsQueryable()
                .Where(o => o.Year == year)
                .ToList();

            return Ok(results);
        }

        [HttpGet]
        [Route("api/oncall/{year}/{week}")]
        public ActionResult GetAllYearWeek(int year, int week)
        {
            var db = new MongoClient(Configuration.mongoServer);
            var database = db.GetDatabase(Configuration.mongoDatabase);
            var collection = database.GetCollection<Oncall>(Configuration.oncallCollection);

            var results = collection.AsQueryable()
                .Where(o => o.Year == year)
                .Where(o => o.Week == week)
                .ToList();

            return Ok(results);
        }

        [HttpGet]
        [Route("api/oncall/team/{team}/{year}")]
        public ActionResult GetTeamYear(string team, int year)
        {
            var db = new MongoClient(Configuration.mongoServer);
            var database = db.GetDatabase(Configuration.mongoDatabase);
            var collection = database.GetCollection<Oncall>(Configuration.oncallCollection);

            var results = collection.AsQueryable()
                .Where(o => o.Year == year)
                .Where(o => o.Team == team)
                .ToList();

            return Ok(results);
        }

        [HttpGet]
        [Route("api/oncall/team/{team}/{year}/{week}")]
        public ActionResult GetTeamYearWeek(string team, int year, int week)
        {
            var db = new MongoClient(Configuration.mongoServer);
            var database = db.GetDatabase(Configuration.mongoDatabase);
            var collection = database.GetCollection<Oncall>(Configuration.oncallCollection);

            var results = collection.AsQueryable()
                .Where(o => o.Year == year)
                .Where(o => o.Week == week)
                .Where(o => o.Team == team)
                .ToList();

            return Ok(results);
        }

        [Route("api/oncall/create/{team}/{year}/{week}/{primary}/{backup}")]
        [HttpGet]
        public ActionResult Create(
            string team,
            int year,
            int week,
            string primary,
            string backup
            )
        {
            try
            {
                var dbClient = new MongoClient(Configuration.mongoServer);
                var database = dbClient.GetDatabase(Configuration.mongoDatabase);
                var colOncall = database.GetCollection<Oncall>(Configuration.oncallCollection);

                var result = colOncall
                .AsQueryable()
                .Where(o => o.Year == year)
                .Where(o => o.Week == week)
                .Where(o => o.Team == team)
                .FirstOrDefault();

                if (result != null)
                {
                    return NotFound("Entry already exists, use update instead...");
                }
                else
                {
                    var oncall = new Oncall()
                    {
                        Id = Guid.NewGuid(),
                        Year = year,
                        Week = week,
                        Team = team,
                        Primary = primary,
                        Backup = backup
                    };

                    colOncall.InsertOne(oncall);

                    return Ok(oncall);
                }
            }
            catch (Exception ex) { BadRequest(ex); }

            return Ok();
        }

        [Route("api/oncall/update/{team}/{year}/{week}/{primary}/{backup}")]
        [HttpGet]
        public ActionResult Update(
            string team,
            int year,
            int week,
            string primary,
            string backup
            )
        {
            try
            {
                var dbClient = new MongoClient(Configuration.mongoServer);
                var database = dbClient.GetDatabase(Configuration.mongoDatabase);
                var colOncall = database.GetCollection<Oncall>(Configuration.oncallCollection);

                var result = colOncall
                .AsQueryable()
                .Where(o => o.Year == year)
                .Where(o => o.Week == week)
                .Where(o => o.Team == team)
                .FirstOrDefault();

                if(result == null)
                {
                    return NotFound("Matching entry not found...");
                }
                else
                {
                    var updateFilter = Builders<Oncall>.Filter.Eq("Year", year);
                    updateFilter &= Builders<Oncall>.Filter.Eq("Week", week);
                    updateFilter &= Builders<Oncall>.Filter.Eq("Team", team);

                    var updateList = new List<UpdateDefinition<Oncall>>();
                    updateList.Add(Builders<Oncall>.Update.Set(o => o.Primary, primary));
                    updateList.Add(Builders<Oncall>.Update.Set(o => o.Backup, backup));

                    var update = Builders<Oncall>.Update.Combine(updateList);

                    colOncall.UpdateOne(updateFilter, update);

                    var test = colOncall.Find(updateFilter).FirstOrDefault();

                    return Ok(test);
                }
            }
            catch(Exception ex) { BadRequest(ex); }

            return Ok();
        }

        [Route("api/oncall/delete/{team}/{year}/{week}")]
        [HttpGet]
        public ActionResult Delete(
            string team,
            int year,
            int week
            )
        {
            try
            {
                var dbClient = new MongoClient(Configuration.mongoServer);
                var database = dbClient.GetDatabase(Configuration.mongoDatabase);
                var colOncall = database.GetCollection<Oncall>(Configuration.oncallCollection);

                var result = colOncall
                .AsQueryable()
                .Where(o => o.Year == year)
                .Where(o => o.Week == week)
                .Where(o => o.Team == team)
                .FirstOrDefault();

                if (result == null)
                {
                    return NotFound("Matching entry not found...");
                }
                else
                {
                    var deleteFilter = Builders<Oncall>.Filter.Eq("Year", year);
                    deleteFilter &= Builders<Oncall>.Filter.Eq("Week", week);
                    deleteFilter &= Builders<Oncall>.Filter.Eq("Team", team);

                    colOncall.DeleteOne(deleteFilter);

                    var test = colOncall.Find(deleteFilter).FirstOrDefault();

                    if(test == null)
                    {
                        return Ok("Record Deleted Successfully...");
                    }
                    else
                    {
                        return BadRequest("Error Deleting Record...");
                    }
                }
            }
            catch (Exception ex) { BadRequest(ex); }

            return Ok();
        }
    }
}
