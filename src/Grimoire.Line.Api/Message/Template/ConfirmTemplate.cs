using System.Collections.Generic;

namespace Grimoire.Line.Api.Message.Template
{
    public record ConfirmTemplate : BaseTemplate
    {
        public ConfirmTemplate()
        {
            TemplateType = TemplateType.Confirm;
        }

        public string Text { get; set; }
        public List<Action.BaseAction> Actions { get; set; } 
    }
}