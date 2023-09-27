﻿using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;
using SearchService.Models;
using SearchService.RequestHelpers;

namespace SearchService.Controllers
{
    [ApiController]
    [Route("api/search")]
    public class SearchController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<Item>>> SearchItems([FromQuery] SearchParams searchParam)
        {
            var query = DB.PagedSearch<Item, Item>();
            //query.Sort(x => x.Ascending(a => a.Make));
            if (!string.IsNullOrEmpty(searchParam.SearchTerm))
            {
                query.Match(Search.Full,searchParam.SearchTerm).SortByTextScore();
            }
            //query = searchParam.OrderBy switch
            //{
            //    "make" => query.Sort(x => x.Ascending(a => a.Make))
            //        .Sort(x => x.Ascending(a => a.Model)),
            //    "new" => query.Sort(x => x.Descending(a => a.CreatedAt)),
            //    _ => query.Sort(x => x.Ascending(a => a.AuctionEnd))
            //};


            //query = searchParam.FilterBy switch
            //{
            //    "finished" => query.Match(x => x.AuctionEnd < DateTime.UtcNow),
            //    "endingSoon" => query.Match(x => x.AuctionEnd < DateTime.UtcNow.AddHours(6)
            //        && x.AuctionEnd > DateTime.UtcNow),
            //    _ => query.Match(x => x.AuctionEnd > DateTime.UtcNow)
            //};

            if (!string.IsNullOrEmpty(searchParam.Seller))
            {
                query.Match(x => x.Seller == searchParam.Seller);
            }

            if (!string.IsNullOrEmpty(searchParam.Winner))
            {
                query.Match(x => x.Winner == searchParam.Winner);
            }


            query.PageNumber(searchParam.PageNumber);
            query.PageSize(searchParam.PageSize);
            var result=await query.ExecuteAsync();
            return Ok(new
            {
                results = result.Results,
                pageCount = result.PageCount,
                totalCount = result.TotalCount
            });
        }
    }
}
