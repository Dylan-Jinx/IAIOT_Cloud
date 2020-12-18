using IAIOT_alpha_0_1_1.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IAIOT_alpha_0_1_1.ResponseControlMsg
{
    public class ResponseCtrMsg<T>
    {
        public ResponseCtrMsg(CtrResult result)
        {
            currentTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            if (result == CtrResult.Success)
            {
                Status = 0;
                message = "操作成功";
            }
            else if(result == CtrResult.Failure)
            {
                Status = 1;
                message = "操作失败";
            }
            else if(result == CtrResult.Unknown)
            {
                Status = 2;
                message = "发生未知错误";
            }
            else if(result == CtrResult.Exception)
            {
                Status = 3;
                message= "出现了异常，详情异常信息请查看ErrorObj";
            }
        }
        public int Status { get; set; }
        public string currentTime;
        public string message { get; set; }
        public T ResultObj { get; set; }
        public Error ErrorObj { get; set; }
    }

}
