﻿using inspirational_quotes_Backend.Models;
using inspirational_quotes_Backend.Services.Repositories.Abstractions;
using Microsoft.AspNetCore.Mvc;
using QuoteModel = inspirational_quotes_Backend.Models.Quote;

namespace inspirational_quotes_Backend.Services.Endpoint
{
    public class QuoteController:ControllerBase
    {
        private IQuoteRepository _quoteRepository;
        public QuoteController(IQuoteRepository quoteRepository)
        {
            _quoteRepository = quoteRepository;
        }
        
        [HttpGet("getquote/{id}")]
        public async Task<IActionResult> GetQuote(int id)
        {
            try
            {
                var quote = await _quoteRepository.GetAsync(id);
                if (quote == null)
                {
                    return NotFound($"Quote with ID {id} not found");
                }
                return Ok(quote);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost("createquotes")]
        public async Task<IActionResult> CreateQuotes([FromBody] List<Quote> quotes)
        {
            try
            {
                if (quotes == null || quotes.Count == 0)
                {
                    return BadRequest("No quotes provided");
                }

                var isSuccess = await _quoteRepository.CreateAsync(quotes);
                if (isSuccess)
                {
                    return Ok("Quotes created successfully");
                }
                return StatusCode(500, "Failed to create quotes");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("updatequote/{id}")]
        public async Task<IActionResult> UpdateQuote([FromBody]Quote quote)
        {
            try
            {
                if (quote == null)
                {
                    return BadRequest("Invalid data provided for update");
                }               
                var isSuccess = await _quoteRepository.UpdateAsync(quote);
                if (isSuccess)
                {
                    return Ok("Quote updated successfully");
                }
                return StatusCode(500, "Failed to update quote");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("deletequote/{id}")]
        public async Task<IActionResult> DeleteQuote(int id)
        {
            try
            {
                var isSuccess = await _quoteRepository.DeleteAsync(id);
                if (!isSuccess)
                {
                    return NotFound($"Quote with ID: {id} not found, deletion failed");
                }
                return Ok("Quote deleted successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("searchquote")]
        public async Task<IActionResult> SearchQuote(string filterName, string filterValue = null)
        {
            try
            {
                var data = await _quoteRepository.GetFilteredQuotesAsync(filterName, filterValue);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("getalltags")]
        public async Task<IActionResult> GetAllTags()
        {
            try
            {
                var Tags = await _quoteRepository.GetAllTagsAsync();
                var TagList = string.Join(",", Tags);
                var data = TagList.Split(',');
                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}