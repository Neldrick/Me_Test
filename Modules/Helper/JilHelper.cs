using Jil;
using System.IO;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Threading.Tasks;
using System;
using System.Text;

namespace MatchingEngine.Modules.Helper
{
    public abstract class JilFormatter
    {
        protected const string CONTENT_TYPE = "application/json";

        protected readonly Options Opts = new Options(excludeNulls: true, includeInherited: true,
                                                      dateFormat: DateTimeFormat.SecondsSinceUnixEpoch,
                                                      serializationNameFormat: SerializationNameFormat.CamelCase);
    }

    // 输入 JSON 解析类
    public class JilInputFormatter : JilFormatter, IInputFormatter
    {
        private readonly Options _options;

        public JilInputFormatter()
            : this(null)
        { }

        public JilInputFormatter(Options options)
        {
            _options = options ?? Opts;
        }

        public bool CanRead(InputFormatterContext context)
        {
            return true;
        }

        public Task<InputFormatterResult> ReadAsync(InputFormatterContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var request = context.HttpContext.Request;

            using (var reader = context.ReaderFactory(request.Body, Encoding.UTF8))
            {
                // 使用 Jil 反序列化
                var result = JSON.Deserialize(reader, context.ModelType, _options);
                return InputFormatterResult.SuccessAsync(result);
            }
        }
    }

    // 输出 JSON 解析类
    public class JilOutputFormatter : JilFormatter, IOutputFormatter
    {
        private readonly Options _options;

        public JilOutputFormatter()
            : this(null)
        { }

        public JilOutputFormatter(Options options)
        {
            _options = options ?? Opts;
        }

        public bool CanWriteResult(OutputFormatterCanWriteContext context)
        {
            return true;
        }

        public Task WriteAsync(OutputFormatterWriteContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var response = context.HttpContext.Response;
            response.ContentType = CONTENT_TYPE;

            if (context.Object == null)
            {
                // 192 好像在 Response.Body 中表示 null
                response.Body.WriteByte(192);
                return Task.CompletedTask;
            }

            using (var writer = context.WriterFactory(response.Body, Encoding.UTF8))
            {
                // 使用 Jil 序列化
                JSON.Serialize(context.Object, writer, _options);
                return Task.CompletedTask;
            }
        }
    }



}