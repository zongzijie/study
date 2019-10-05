# 在urllib 库里面,查找Python的request模块，只导入一个urlopen函数
from urllib.request import urlopen
# 导入我们刚才安装的BeautifulSoup对象
from bs4 import BeautifulSoup
html=urlopen('http://jr.jd.com') # 打开url,获取html内容
# 把html内容传到BeautifulSoup对象
bs_obj=BeautifulSoup(html.read(),'html.parser')
# 找到所有class="nav-item-primary"的a标签
text_list=bs_obj.find_all("a","nav-item-primary")
for text in text_list:
	print(text.get_text()) # 打印标签的文本
html.close() # 关闭文件