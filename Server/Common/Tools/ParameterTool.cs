using LitJson;
using System;
using System.Collections.Generic;
using System.Text;

namespace GodCommon.Tools
{
    public class ParameterTool  //参数模板
    {
        public static T GetParameter<T>(Dictionary<byte, object> parameters, ParameterCode parameterCode, bool isObject = true)
        {
            object o = null;
            parameters.TryGetValue((byte)parameterCode, out o);
            if (isObject)
            {
                return JsonMapper.ToObject<T>(o.ToString());
            }
            else
            {
                return (T)o;
            }
        }

        public static void AddParameter<T>(Dictionary<byte, object> parameters, ParameterCode key, T value, bool isObject = true)
        {
            if (isObject)
            {
                string json = JsonMapper.ToJson(value);
                parameters.Add((byte)key, json);
            }
            else
            {
                parameters.Add((byte)key, value);
            }
        }

        public static SubCode GetSubCode(Dictionary<byte, object> parameters)
        {
            return GetParameter<SubCode>(parameters, ParameterCode.SubCode, false);
        }

        public static void AddSubCode(Dictionary<byte, object> parameters, SubCode subCode)
        {
            AddParameter(parameters, ParameterCode.SubCode, subCode, false);
        }

        public static void AddOperationCode(Dictionary<byte, object> parameters, OperationCode opCode)
        {
            AddParameter(parameters, ParameterCode.OperationCode, opCode, false);
        }

        public static void AddEventToPeer(Dictionary<byte, object> parameters, OperationCode opCode, SubCode subCode, int roleId)
        {
            if (parameters.ContainsKey((byte)ParameterCode.OperationCode) == false)  //没有key才需要添加
            {
                parameters.Add((byte)ParameterCode.OperationCode, opCode);
            }
            if (parameters.ContainsKey((byte)ParameterCode.SubCode) == false)
            {
                parameters.Add((byte)ParameterCode.SubCode, subCode);
            }
            if (parameters.ContainsKey((byte)ParameterCode.RoleId) == false)
            {
                parameters.Add((byte)ParameterCode.RoleId, roleId);
            }
        }
    }
}
