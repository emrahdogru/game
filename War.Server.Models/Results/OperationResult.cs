using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace War.Server.Models.Results
{
    public class OperationResult<IObject>(IObject? result) where IObject: class
    {
        public bool Success { get; init; }
        public string? Message { get; init; } = null;
        public IEnumerable<string>? Errors { get; init; } = null;
        public IEnumerable<string>? Warnings { get; init; } = null;
        public IObject? Result { get; init; } = result;
    }
}
