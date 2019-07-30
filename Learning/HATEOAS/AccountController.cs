using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
using System.Collections.Generic;
using System.Linq;

namespace Learning.HATEOAS
{
    [ApiController]
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        private readonly IUrlHelper _urlHelper;

        public AccountController(IUrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }

        [HttpGet("/api/accounts/", Name = "GetAccounts")]
        public IActionResult GetAccounts()
        {
            var result = new List<AccountDto> { new AccountDto { BSB = "1234", Number = "2342" }, new AccountDto { BSB = "234322", Number = "34324" } };

            result = result.Select(CreateAccountLinks).ToList();

            var wrapper = new LinkedCollectionResourceWrapperDto<AccountDto>(result);

            return Ok(CreateAccountsLinks(wrapper));
        }

        private LinkedCollectionResourceWrapperDto<AccountDto> CreateAccountsLinks(LinkedCollectionResourceWrapperDto<AccountDto> wrapper)
        {
            // link to self
            wrapper.Links.Add(
                new LinkDto(_urlHelper.Link("GetAccounts", new { }),
                "self",
                "GET"));

            return wrapper;
        }

        [HttpGet("/api/account/{accountNumber}", Name = "GetAccount")]
        public IActionResult Get([FromRoute] string accountNumber)
        {
            var result = new AccountDto();
            result = CreateAccountLinks(result);

            return Ok(result);
        }

        [HttpDelete("/api/account/{accountNumber}", Name = "DeleteAccount")]
        public IActionResult Delete([FromRoute] string accountNumber)
        {
            return NoContent();
        }

        [HttpPut("/api/account/{accountNumber}", Name = "UpdateAccount")]
        public IActionResult Update([FromRoute] string accountNumber, AccountDto account)
        {
            return CreatedAtRoute("GetAcount",
               new { id = accountNumber },
               account);
        }

        private LinkedCollectionResourceWrapperDto<AccountDto> CreateLinksForAccounts(
           LinkedCollectionResourceWrapperDto<AccountDto> accountsWrapper)
        {
            // link to self
            accountsWrapper.Links.Add(
                new LinkDto(_urlHelper.Link("GetAccounts", new { }),
                "self",
                "GET"));

            return accountsWrapper;
        }

        private AccountDto CreateAccountLinks(AccountDto result)
        {
            result.Links.Add(
                new LinkDto(_urlHelper.Link("GetAccount",
                new { id = result.Number }),
                    "self",
                HttpMethod.Get.ToString()));

            result.Links.Add(
                new LinkDto(_urlHelper.Link("DeleteAccount",
                new { id = result.Number }),
                "delete_account",
                HttpMethod.Delete.ToString()));

            result.Links.Add(
                new LinkDto(_urlHelper.Link("UpdateAccount",
                new { id = result.Number }),
                "update_account",
                  HttpMethod.Put.ToString()));

            return result;
        }
    }
}
