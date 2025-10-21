using NLog;

namespace SCPIMCMain.Model.Service;

/// <summary>
/// NLog를 사용한 로깅 서비스
/// Singleton 패턴으로 전역적으로 사용 가능
/// </summary>
public class LoggingService
{
    private readonly ILogger _logger;

    /// <summary>
    /// 기본 생성자 - 현재 클래스 이름으로 Logger 생성
    /// </summary>
    public LoggingService()
    {
        _logger = LogManager.GetCurrentClassLogger();
    }

    /// <summary>
    /// 특정 Logger 이름으로 생성
    /// </summary>
    /// <param name="__loggerName">Logger 이름</param>
    public LoggingService(string __loggerName)
    {
        if (string.IsNullOrEmpty(__loggerName))
        {
            _logger = LogManager.GetCurrentClassLogger();
        }
        else
        {
            _logger = LogManager.GetLogger(__loggerName);
        }
    }

    // ===== Trace Level =====

    /// <summary>
    /// Trace 레벨 로그 기록
    /// 가장 상세한 정보, 일반적으로 프로덕션에서는 사용하지 않음
    /// </summary>
    /// <param name="__message">로그 메시지</param>
    public void Func_Trace(string __message)
    {
        _logger.Trace(__message);
    }

    /// <summary>
    /// Trace 레벨 로그 기록 (예외 포함)
    /// </summary>
    /// <param name="__message">로그 메시지</param>
    /// <param name="__exception">예외 객체</param>
    public void Func_Trace(string __message, Exception __exception)
    {
        _logger.Trace(__exception, __message);
    }

    // ===== Debug Level =====

    /// <summary>
    /// Debug 레벨 로그 기록
    /// 디버깅에 유용한 정보, 개발 환경에서 주로 사용
    /// </summary>
    /// <param name="__message">로그 메시지</param>
    public void Func_Debug(string __message)
    {
        _logger.Debug(__message);
    }

    /// <summary>
    /// Debug 레벨 로그 기록 (예외 포함)
    /// </summary>
    /// <param name="__message">로그 메시지</param>
    /// <param name="__exception">예외 객체</param>
    public void Func_Debug(string __message, Exception __exception)
    {
        _logger.Debug(__exception, __message);
    }

    // ===== Info Level =====

    /// <summary>
    /// Info 레벨 로그 기록
    /// 일반적인 정보성 메시지
    /// </summary>
    /// <param name="__message">로그 메시지</param>
    public void Func_Info(string __message)
    {
        _logger.Info(__message);
    }

    /// <summary>
    /// Info 레벨 로그 기록 (예외 포함)
    /// </summary>
    /// <param name="__message">로그 메시지</param>
    /// <param name="__exception">예외 객체</param>
    public void Func_Info(string __message, Exception __exception)
    {
        _logger.Info(__exception, __message);
    }

    // ===== Warn Level =====

    /// <summary>
    /// Warn 레벨 로그 기록
    /// 경고 - 잠재적인 문제 상황
    /// </summary>
    /// <param name="__message">로그 메시지</param>
    public void Func_Warn(string __message)
    {
        _logger.Warn(__message);
    }

    /// <summary>
    /// Warn 레벨 로그 기록 (예외 포함)
    /// </summary>
    /// <param name="__message">로그 메시지</param>
    /// <param name="__exception">예외 객체</param>
    public void Func_Warn(string __message, Exception __exception)
    {
        _logger.Warn(__exception, __message);
    }

    // ===== Error Level =====

    /// <summary>
    /// Error 레벨 로그 기록
    /// 에러 - 기능 수행 실패, 하지만 애플리케이션은 계속 실행
    /// </summary>
    /// <param name="__message">로그 메시지</param>
    public void Func_Error(string __message)
    {
        _logger.Error(__message);
    }

    /// <summary>
    /// Error 레벨 로그 기록 (예외 포함)
    /// </summary>
    /// <param name="__message">로그 메시지</param>
    /// <param name="__exception">예외 객체</param>
    public void Func_Error(string __message, Exception __exception)
    {
        _logger.Error(__exception, __message);
    }

    // ===== Fatal Level =====

    /// <summary>
    /// Fatal 레벨 로그 기록
    /// 치명적 오류 - 애플리케이션 종료를 야기할 수 있는 오류
    /// </summary>
    /// <param name="__message">로그 메시지</param>
    public void Func_Fatal(string __message)
    {
        _logger.Fatal(__message);
    }

    /// <summary>
    /// Fatal 레벨 로그 기록 (예외 포함)
    /// </summary>
    /// <param name="__message">로그 메시지</param>
    /// <param name="__exception">예외 객체</param>
    public void Func_Fatal(string __message, Exception __exception)
    {
        _logger.Fatal(__exception, __message);
    }

    // ===== Utility Methods =====

    /// <summary>
    /// 특정 로그 레벨이 활성화되어 있는지 확인
    /// </summary>
    /// <param name="__level">확인할 로그 레벨</param>
    /// <returns>활성화 여부</returns>
    public bool Func_IsEnabled(LogLevel __level)
    {
        return _logger.IsEnabled(__level);
    }

    /// <summary>
    /// 현재 Logger 이름 반환
    /// </summary>
    /// <returns>Logger 이름</returns>
    public string Func_GetLoggerName()
    {
        return _logger.Name;
    }

    /// <summary>
    /// 로그 파일을 즉시 플러시 (버퍼에 있는 내용을 파일에 기록)
    /// </summary>
    public void Func_Flush()
    {
        LogManager.Flush();
    }

    /// <summary>
    /// NLog 종료 (애플리케이션 종료 시 호출)
    /// </summary>
    public static void Func_Shutdown()
    {
        LogManager.Shutdown();
    }
}
