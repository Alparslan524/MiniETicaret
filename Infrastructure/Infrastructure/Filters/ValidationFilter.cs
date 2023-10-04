using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Filters
{
    public class ValidationFilter : IAsyncActionFilter //Kendimize özgü filtre
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)//geçersiz bir durum söz konusuysa
            {
                var errors = context.ModelState
                       .Where(x => x.Value.Errors.Any())
                       .ToDictionary(e => e.Key, e => e.Value.Errors.Select(e => e.ErrorMessage))
                       .ToArray();
                context.Result = new BadRequestObjectResult(errors);
                return;
            }
            await next();
        }
        //Her istek yapıldığında bu filtreye takılacak. Eğer geçersiz bir durum var ise yani validationa takılan bir durum var ise
        //context.ModelState.IsValid false olacak ve if içine girecek. İf içinde hataları ayıklıyor ve ne kadar hata var ise frontende geri gönderiyor.
    }
}
