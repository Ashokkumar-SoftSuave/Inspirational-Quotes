using inspirational_quotes_Backend.Models;
using inspirational_quotes_Backend.Services.Endpoint;
using inspirational_quotes_Backend.Services.Repositories.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework.Legacy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InspirationalQuotes_Test
{
    [TestFixture]
    public class QuoteTests
    {
        private Mock<IQuoteRepository> _quoteRepositoryMock;
        private QuoteController _quoteController;

        [SetUp]
        //public void Setup()
        //{
        //    _quoteRepositoryMock = new Mock<IQuoteRepository>();
        //    _quoteController = new QuoteController(_quoteRepositoryMock.Object);
        //}

        #region Get

        [Test]
        public async Task GetQuote_WithValidId_ReturnsOkResponse()
        {
            // Arrange
            var quoteId = 1;
            var quote = new Quote { Id = quoteId, Author = "John Doe", QuoteDesp = "Description" };
            _quoteRepositoryMock.Setup(repo => repo.GetAsync(quoteId)).ReturnsAsync(quote);

            // Act
            var result = await _quoteController.GetQuote(quoteId);

            // Assert
            var okResult = result as OkObjectResult;
            ClassicAssert.IsNotNull(okResult);
            ClassicAssert.AreEqual(200, okResult.StatusCode);
            ClassicAssert.AreEqual(quote, okResult.Value);
        }

        [Test]
        public async Task GetQuote_WithInvalidId_ReturnsNotFoundResponse()
        {
            // Arrange
            var nonExistentQuoteId = 999;
            _quoteRepositoryMock.Setup(repo => repo.GetAsync(nonExistentQuoteId)).ReturnsAsync(null as Quote);

            // Act
            var result = await _quoteController.GetQuote(nonExistentQuoteId);

            // Assert
            var notFoundResult = result as NotFoundObjectResult;
            ClassicAssert.IsNotNull(notFoundResult);
            ClassicAssert.AreEqual(404, notFoundResult.StatusCode);
            ClassicAssert.AreEqual($"Quote with ID {nonExistentQuoteId} not found", notFoundResult.Value);
        }

        [Test]
        public async Task GetQuote_ThrowsInternalServerError_ReturnsInternalServerErrorResponse()
        {
            // Arrange
            var quoteId = 1;
            _quoteRepositoryMock.Setup(repo => repo.GetAsync(quoteId)).ThrowsAsync(new Exception("Error getting quote"));

            // Act
            var result = await _quoteController.GetQuote(quoteId);

            // Assert
            var internalServerErrorResult = result as ObjectResult;
            ClassicAssert.IsNotNull(internalServerErrorResult);
            ClassicAssert.AreEqual(500, internalServerErrorResult.StatusCode);
            ClassicAssert.AreEqual("Error getting quote", internalServerErrorResult.Value);
        }

        #endregion

        #region Create Quote Test Cases
        [Test]
        public async Task CreateQuotes_WithValidData_ReturnsOkResponse()
        {
            // Arrange
            var quotes = new List<Quote>
            {
                new Quote { Id = 1, Author = "Author 1", Tags = "Tag 1", QuoteDesp = "Description 1" },
                new Quote { Id = 2, Author = "Author 2", Tags = "Tag 2", QuoteDesp = "Description 2" }
            };
            _quoteRepositoryMock.Setup(repo => repo.CreateAsync(quotes)).ReturnsAsync(true);

            // Act
            var result = await _quoteController.CreateQuotes(quotes);

            // Assert
            var okResult = result as OkObjectResult;
            ClassicAssert.IsNotNull(okResult);
            ClassicAssert.AreEqual(200, okResult.StatusCode);
            ClassicAssert.AreEqual("Quotes created successfully", okResult.Value);
        }

        [Test]
        public async Task CreateQuotes_WithNullQuotes_ReturnsBadRequestResponse()
        {
            // Arrange
            List<Quote> quotes = null;

            // Act
            var result = await _quoteController.CreateQuotes(quotes);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            ClassicAssert.IsNotNull(badRequestResult);
            ClassicAssert.AreEqual(400, badRequestResult.StatusCode);
            ClassicAssert.AreEqual("No quotes provided", badRequestResult.Value);
        }

        [Test]
        public async Task CreateQuotes_WithEmptyQuotes_ReturnsBadRequestResponse()
        {
            // Arrange
            List<Quote> quotes = new List<Quote>();

            // Act
            var result = await _quoteController.CreateQuotes(quotes);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            ClassicAssert.IsNotNull(badRequestResult);
            ClassicAssert.AreEqual(400, badRequestResult.StatusCode);
            ClassicAssert.AreEqual("No quotes provided", badRequestResult.Value);
        }
        #endregion
        #region Update Quote Test Cases
        [Test]
        public async Task UpdateQuote_WithValidData_ReturnsOkResponse()
        {
            // Arrange
            var quoteId = 1;
            var updatedQuote = new Quote { Id = quoteId, Author = "Updated Author", Tags = "test Tags", QuoteDesp = "Updated Quote" };
            _quoteRepositoryMock.Setup(repo => repo.GetAsync(quoteId)).ReturnsAsync(updatedQuote);
            _quoteRepositoryMock.Setup(repo => repo.UpdateAsync(updatedQuote)).ReturnsAsync(true);

            // Act
            var result = await _quoteController.UpdateQuote(updatedQuote);

            // Assert
            var okResult = result as OkObjectResult;
            ClassicAssert.IsNotNull(okResult);
            ClassicAssert.AreEqual(200, okResult.StatusCode);
            ClassicAssert.AreEqual("Quote updated successfully", okResult.Value);
        }

        [Test]
        public async Task UpdateQuote_WithInvalidData_ReturnsBadRequestResponse()
        {
            // Arrange
            Quote invalidQuote = null;

            // Act
            var result = await _quoteController.UpdateQuote(invalidQuote);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            ClassicAssert.IsNotNull(badRequestResult);
            ClassicAssert.AreEqual(400, badRequestResult.StatusCode);
            ClassicAssert.AreEqual("Invalid data provided for update", badRequestResult.Value);
        }

        [Test]
        public async Task UpdateQuote_WhenUpdateFails_ReturnsInternalServerErrorResponse()
        {
            // Arrange
            var quoteId = 1;
            var existingQuote = new Quote { Id = quoteId };
            _quoteRepositoryMock.Setup(repo => repo.GetAsync(quoteId)).ReturnsAsync(existingQuote);
            _quoteRepositoryMock.Setup(repo => repo.UpdateAsync(existingQuote)).ReturnsAsync(false);

            // Act
            var result = await _quoteController.UpdateQuote(existingQuote);

            // Assert
            var internalServerErrorResult = result as ObjectResult;
            ClassicAssert.IsNotNull(internalServerErrorResult);
            ClassicAssert.AreEqual(500, internalServerErrorResult.StatusCode);
            ClassicAssert.AreEqual("Failed to update quote", internalServerErrorResult.Value);
        }
        #endregion
        #region Delete Quote Test Cases
        //[Test]
        //public async Task DeleteQuote_WithValidId_ReturnsOkResponse()
        //{
        //    // Arrange
        //    var quoteId = 1;
        //    _quoteRepositoryMock.Setup(repo => repo.DeleteAsync(quoteId)).ReturnsAsync(true);

        //    // Act
        //    var result = await _quoteController.DeleteQuote(quoteId);

        //    // Assert
        //    var okResult = result as OkObjectResult;
        //    ClassicAssert.IsNotNull(okResult);
        //    ClassicAssert.AreEqual(200, okResult.StatusCode);
        //    ClassicAssert.AreEqual("Quote deleted successfully", okResult.Value);
        //}

        //[Test]
        //public async Task DeleteQuote_WithInvalidId_ReturnsNotFoundResponse()
        //{
        //    // Arrange
        //    var nonExistentQuoteId = 999;
        //    _quoteRepositoryMock.Setup(repo => repo.DeleteAsync(nonExistentQuoteId)).ReturnsAsync(false);

        //    // Act
        //    var result = await _quoteController.DeleteQuote(nonExistentQuoteId);

        //    // Assert
        //    var notFoundResult = result as NotFoundObjectResult;
        //    ClassicAssert.IsNotNull(notFoundResult);
        //    ClassicAssert.AreEqual(404, notFoundResult.StatusCode);
        //    ClassicAssert.AreEqual($"Quote with ID: {nonExistentQuoteId} not found, deletion failed", notFoundResult.Value);
        //}

        //[Test]
        //public async Task DeleteQuote_ThrowsInternalServerError_ReturnsInternalServerErrorResponse()
        //{
        //    // Arrange
        //    var quoteId = 1;
        //    _quoteRepositoryMock.Setup(repo => repo.DeleteAsync(quoteId)).ThrowsAsync(new Exception("Error deleting quote"));

        //    // Act
        //    var result = await _quoteController.DeleteQuote(quoteId);

        //    // Assert
        //    var internalServerErrorResult = result as ObjectResult;
        //    ClassicAssert.IsNotNull(internalServerErrorResult);
        //    ClassicAssert.AreEqual(500, internalServerErrorResult.StatusCode);
        //    ClassicAssert.AreEqual("Error deleting quote", internalServerErrorResult.Value);
        //}
        #endregion

        #region search


        //[Test]
        //public async Task SearchQuote_WithValidFilterNameAndValue_ReturnsOkResponse()
        //{
        //    // Arrange
        //    var filterName = "Author";
        //    var filterValue = "John Doe";
        //    var filteredQuotes = new List<Quote>
        //    {
        //        new Quote { Id = 1, Author = "John Doe", QuoteDesp = "Description 1" },
        //        new Quote { Id = 2, Author = "John Doe", QuoteDesp = "Description 2" }
        //    };
        //    _quoteRepositoryMock.Setup(repo => repo.GetFilteredQuotesAsync(filterName, filterValue)).ReturnsAsync(filteredQuotes);

        //    // Act
        //    var result = await _quoteController.SearchQuote(filterName, filterValue);

        //    // Assert
        //    var okResult = result as OkObjectResult;
        //    ClassicAssert.IsNotNull(okResult);
        //    ClassicAssert.AreEqual(200, okResult.StatusCode);
        //    ClassicAssert.AreEqual(filteredQuotes, okResult.Value);
        //}

        [Test]
        public async Task SearchQuote_ThrowsInternalServerError_ReturnsInternalServerErrorResponse()
        {
            // Arrange
            var filterName = "Author";
            var filterValue = "John Doe";
            _quoteRepositoryMock.Setup(repo => repo.GetFilteredQuotesAsync(filterName, filterValue)).ThrowsAsync(new Exception("Error searching quotes"));

            // Act
            //var result = await _quoteController.SearchQuote(filterName, filterValue);

            //// Assert
            //var internalServerErrorResult = result as ObjectResult;
            //ClassicAssert.IsNotNull(internalServerErrorResult);
            //ClassicAssert.AreEqual(500, internalServerErrorResult.StatusCode);
            //ClassicAssert.AreEqual("Error searching quotes", internalServerErrorResult.Value);
        }

        #endregion
    }
}
