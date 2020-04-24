# ZeroAllocationStrings

```csharp
for(int i=0;i<1000;i++) 
  string newZeroGcString = NonAllocString.instance+"player HP:"+23;
```

by adding NonAllocString.instance prefix, string will be cached, so it will be allocated only 1 time
