using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using MyFixIt.Common.Models;

namespace MyFixIt
{
    public interface ITaskService
    {
        Task<List<FixItTask>> ListByCreatorAsync(string creator);
        Task CreateAsync(FixItTask task, HttpPostedFileBase photo);
    }
}