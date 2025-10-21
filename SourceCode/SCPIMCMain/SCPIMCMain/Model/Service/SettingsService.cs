using System.IO;
using Newtonsoft.Json;
using SCPIMCMain.Model.Implementation;

namespace SCPIMCMain.Model.Service
{
    /// <summary>
    /// 애플리케이션 설정의 저장 및 로드를 담당하는 서비스 클래스
    /// </summary>
    public class SettingsService
    {
        /// <summary>
        /// 설정을 파일에 저장합니다.
        /// </summary>
        /// <param name="__settings">저장할 설정 객체</param>
        public static void Func_SaveSettings(ApplicationSettings __settings)
        {
            try
            {
                if (__settings == null)
                {
                    throw new ArgumentNullException(nameof(__settings), "Settings cannot be null.");
                }

                string file_path = FileSaverAndLoaderService.Func_GetSettingsFilePath();
                string directory_path = Path.GetDirectoryName(file_path);

                // Settings 디렉토리가 없으면 생성
                if (!string.IsNullOrEmpty(directory_path) && !Directory.Exists(directory_path))
                {
                    Directory.CreateDirectory(directory_path);
                }

                // JSON으로 직렬화하여 저장
                string json_content = JsonConvert.SerializeObject(__settings, Formatting.Indented);
                File.WriteAllText(file_path, json_content);

                Console.WriteLine($"Settings saved to {file_path}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving settings: {ex.Message}");
            }
        }

        /// <summary>
        /// 파일에서 설정을 로드합니다.
        /// </summary>
        /// <returns>로드된 설정 객체. 파일이 없거나 오류 발생 시 기본 설정 반환</returns>
        public static ApplicationSettings Func_LoadSettings()
        {
            try
            {
                string file_path = FileSaverAndLoaderService.Func_GetSettingsFilePath();

                // 파일이 없으면 기본 설정 반환
                if (!File.Exists(file_path))
                {
                    Console.WriteLine($"Settings file not found. Using default settings.");
                    return new ApplicationSettings();
                }

                // JSON 파일 읽어서 역직렬화
                string json_content = File.ReadAllText(file_path);
                ApplicationSettings settings = JsonConvert.DeserializeObject<ApplicationSettings>(json_content);

                if (settings == null)
                {
                    Console.WriteLine("Failed to deserialize settings. Using default settings.");
                    return new ApplicationSettings();
                }

                Console.WriteLine($"Settings loaded from {file_path}");
                return settings;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading settings: {ex.Message}. Using default settings.");
                return new ApplicationSettings();
            }
        }
    }
}
