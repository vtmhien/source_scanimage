using System;
using System.Linq;
using xNet;

namespace ScanimageGroup
{
	// Token: 0x02000019 RID: 25
	public class Req
	{
		// Token: 0x060000DA RID: 218 RVA: 0x00009C18 File Offset: 0x00007E18
		public Req(string cookie = "", string userAgent = "", string proxy = "", int typeProxy = 0)
		{
			bool flag = userAgent == "";
			if (flag)
			{
				userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.131 Safari/537.36";
			}
			this.request = new HttpRequest
			{
				KeepAlive = true,
				AllowAutoRedirect = true,
				Cookies = new CookieDictionary(false),
				UserAgent = userAgent
			};
			this.request.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8");
			this.request.AddHeader("Accept-Language", "en-US,en;q=0.9");
			bool flag2 = cookie != "";
			if (flag2)
			{
				this.AddCookie(cookie);
			}
			bool flag3 = proxy != "";
			if (flag3)
			{
				switch (proxy.Split(new char[]
				{
					':'
				}).Count<string>())
				{
					case 1:
						this.request.Proxy = Socks5ProxyClient.Parse("127.0.0.1:" + proxy);
						break;
					case 2:
						{
							bool flag4 = typeProxy == 0;
							if (flag4)
							{
								this.request.Proxy = HttpProxyClient.Parse(proxy);
							}
							else
							{
								this.request.Proxy = Socks5ProxyClient.Parse(proxy);
							}
							break;
						}
					case 4:
						this.request.Proxy = new HttpProxyClient(proxy.Split(new char[]
						{
						':'
						})[0], Convert.ToInt32(proxy.Split(new char[]
						{
						':'
						})[1]), proxy.Split(new char[]
						{
						':'
						})[2], proxy.Split(new char[]
						{
						':'
						})[3]);
						break;
				}
			}
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00009DC4 File Offset: 0x00007FC4
		public string RequestGet(string url)
		{
			return this.request.Get(url, null).ToString();
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00009DE8 File Offset: 0x00007FE8
		public byte[] GetBytes(string url)
		{
			return this.request.Get(url, null).ToBytes();
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00009E0C File Offset: 0x0000800C
		public string RequestPost(string url, string data = "", string contentType = "application/x-www-form-urlencoded")
		{
			bool flag = data == "" || contentType == "";
			string result;
			if (flag)
			{
				result = this.request.Post(url).ToString();
			}
			else
			{
				result = this.request.Post(url, data, contentType).ToString();
			}
			return result;
		}

		// Token: 0x060000DE RID: 222 RVA: 0x00009E64 File Offset: 0x00008064
		public void AddCookie(string cookie)
		{
			string[] array = cookie.Split(new char[]
			{
				';'
			});
			foreach (string text in array)
			{
				string[] array3 = text.Split(new char[]
				{
					'='
				});
				bool flag = array3.Count<string>() > 1;
				if (flag)
				{
					try
					{
						this.request.Cookies.Add(array3[0], array3[1]);
					}
					catch
					{
					}
				}
			}
		}

		// Token: 0x060000DF RID: 223 RVA: 0x00009EF4 File Offset: 0x000080F4
		public string GetCookie()
		{
			return this.request.Cookies.ToString();
		}

		// Token: 0x04000058 RID: 88
		private HttpRequest request;
	}
}
