# Zero Allocation Strings

```csharp
for(int i=0;i<1000;i++) 
{
  string newZeroGcString = NonAllocString.instance+"player HP:"+100;
}
```

by adding NonAllocString.instance prefix, string will be cached, so it will be allocated only 1 time

Did some optimization and now its even faster than https://github.com/Cysharp/ZString

```c#
    public void Update()
    {
        Utf16ValueStringBuilder st = ZString.CreateStringBuilder();
        using (bs.Profile("ZString"))
            for (int i = 0; i < 1000; i++)
            {
                    st.Append("foo");
                    st.AppendLine(i);
                    st.AppendLine(i);
                    var str = st.ToString();
                    st.Clear();
            }
        var sb = new StringBuilder();
        using (bs.Profile("StringBuilder"))
            for (int i = 0; i < 1000; i++)
            {
                sb.Append("foo");
                sb.Append(i);
                sb.Append(i);
                var str = sb.ToString();
                sb.Clear();
            }

        using (bs.Profile("NonAllocString"))
        for(int i=0;i<1000;i++) 
        {
            string newZeroGcString = NonAllocString.instance + "foo" +i + i;
        }

    }
```

![](https://user-images.githubusercontent.com/16543239/118136314-a96c8200-b40c-11eb-94ad-53991b3d3456.png)
