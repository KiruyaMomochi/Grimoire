using System;
using System.Collections.Generic;

namespace Grimoire.LineApi.Things
{
    public record ScenarioResult
    {
        public string ScenarioId { get; set; }
        public int Revision { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public ResultCode ResultCode { get; set; }
        public string BleNotificationPayload { get; set; }
        public List<ActionResult> ActionResults { get; set; }
    }
}