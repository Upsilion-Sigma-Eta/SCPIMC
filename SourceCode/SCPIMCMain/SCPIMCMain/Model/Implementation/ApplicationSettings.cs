using Newtonsoft.Json;

namespace SCPIMCMain.Model.Implementation
{
    /// <summary>
    /// 애플리케이션 전역 설정을 저장하는 모델 클래스
    /// </summary>
    public class ApplicationSettings
    {
        /// <summary>
        /// 마지막으로 사용한 장치의 IP 주소
        /// </summary>
        [JsonProperty("last_device_ip")]
        public string LastDeviceIp { get; set; }

        /// <summary>
        /// 마지막으로 사용한 장치의 포트 번호
        /// </summary>
        [JsonProperty("last_device_port")]
        public int LastDevicePort { get; set; }

        /// <summary>
        /// 기본 생성자 - 기본값으로 초기화
        /// </summary>
        public ApplicationSettings()
        {
            LastDeviceIp = "0.0.0.0";
            LastDevicePort = 0;
        }

        /// <summary>
        /// 매개변수를 받는 생성자
        /// </summary>
        /// <param name="__ipAddress">IP 주소</param>
        /// <param name="__port">포트 번호</param>
        public ApplicationSettings(string __ipAddress, int __port)
        {
            LastDeviceIp = __ipAddress;
            LastDevicePort = __port;
        }
    }
}
