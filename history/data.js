window.BENCHMARK_DATA = {
  "lastUpdate": 1792403020807,
  "repoUrl": "https://github.com/assimalign/cohesion-benchmarks",
  "entries": {
    "libraries/ApplicationModel - time": [
      {
        "commit": {
          "author": {
            "name": "Chase Crawford",
            "username": "dotnetcadet",
            "email": "56458944+dotnetcadet@users.noreply.github.com"
          },
          "committer": {
            "name": "GitHub",
            "username": "web-flow",
            "email": "noreply@github.com"
          },
          "id": "3861cd040532be00d816c57a1fc14b242b9195a4",
          "message": "Merge pull request #4 from assimalign/fix/benchmarks-filter-glob\n\nfix(benchmarks): stop PowerShell globbing the filter on Linux; fail runs that measure nothing",
          "timestamp": "2026-10-19T09:36:35Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/3861cd040532be00d816c57a1fc14b242b9195a4"
        },
        "date": 1792402973894,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "GenericPlannerBenchmarks.CreatePlan",
            "value": 1276.732,
            "range": "± 14.859",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079860\nmean 1280.421 ns"
          },
          {
            "name": "GenericPlannerBenchmarks.ValidatePlan",
            "value": 5629.211,
            "range": "± 57.153",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079860\nmean 5648.242 ns"
          },
          {
            "name": "ResourceManifestBenchmarks.Parse",
            "value": 8691.089,
            "range": "± 36.44",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079860\nmean 8684.142 ns"
          },
          {
            "name": "ResourceManifestBenchmarks.ComputeHash",
            "value": 26258.138,
            "range": "± 210.718",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079860\nmean 26273.064 ns"
          }
        ]
      }
    ],
    "libraries/ApplicationModel - allocations": [
      {
        "commit": {
          "author": {
            "name": "Chase Crawford",
            "username": "dotnetcadet",
            "email": "56458944+dotnetcadet@users.noreply.github.com"
          },
          "committer": {
            "name": "GitHub",
            "username": "web-flow",
            "email": "noreply@github.com"
          },
          "id": "3861cd040532be00d816c57a1fc14b242b9195a4",
          "message": "Merge pull request #4 from assimalign/fix/benchmarks-filter-glob\n\nfix(benchmarks): stop PowerShell globbing the filter on Linux; fail runs that measure nothing",
          "timestamp": "2026-10-19T09:36:35Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/3861cd040532be00d816c57a1fc14b242b9195a4"
        },
        "date": 1792402975724,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "GenericPlannerBenchmarks.CreatePlan - allocated",
            "value": 3512,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079860"
          },
          {
            "name": "GenericPlannerBenchmarks.ValidatePlan - allocated",
            "value": 14353,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079860"
          },
          {
            "name": "ResourceManifestBenchmarks.Parse - allocated",
            "value": 5208,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079860"
          },
          {
            "name": "ResourceManifestBenchmarks.ComputeHash - allocated",
            "value": 33026,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079860"
          }
        ]
      }
    ],
    "libraries/Cache - time": [
      {
        "commit": {
          "author": {
            "name": "Chase Crawford",
            "username": "dotnetcadet",
            "email": "56458944+dotnetcadet@users.noreply.github.com"
          },
          "committer": {
            "name": "GitHub",
            "username": "web-flow",
            "email": "noreply@github.com"
          },
          "id": "3861cd040532be00d816c57a1fc14b242b9195a4",
          "message": "Merge pull request #4 from assimalign/fix/benchmarks-filter-glob\n\nfix(benchmarks): stop PowerShell globbing the filter on Linux; fail runs that measure nothing",
          "timestamp": "2026-10-19T09:36:35Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/3861cd040532be00d816c57a1fc14b242b9195a4"
        },
        "date": 1792402989205,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "MemoryCacheBenchmarks.TryGetValue_MicrosoftExtensions",
            "value": 29.917,
            "range": "± 0.348",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079861\nmean 29.824 ns"
          },
          {
            "name": "MemoryCacheBenchmarks.TryGetValue",
            "value": 56.473,
            "range": "± 0.829",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079861\nmean 56.65 ns"
          },
          {
            "name": "MemoryCacheBenchmarks.Set_MicrosoftExtensions",
            "value": 57.544,
            "range": "± 0.464",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079861\nmean 57.557 ns"
          },
          {
            "name": "MemoryCacheBenchmarks.Set",
            "value": 95.53,
            "range": "± 3.275",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079861\nmean 95.556 ns"
          }
        ]
      }
    ],
    "libraries/Cache - allocations": [
      {
        "commit": {
          "author": {
            "name": "Chase Crawford",
            "username": "dotnetcadet",
            "email": "56458944+dotnetcadet@users.noreply.github.com"
          },
          "committer": {
            "name": "GitHub",
            "username": "web-flow",
            "email": "noreply@github.com"
          },
          "id": "3861cd040532be00d816c57a1fc14b242b9195a4",
          "message": "Merge pull request #4 from assimalign/fix/benchmarks-filter-glob\n\nfix(benchmarks): stop PowerShell globbing the filter on Linux; fail runs that measure nothing",
          "timestamp": "2026-10-19T09:36:35Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/3861cd040532be00d816c57a1fc14b242b9195a4"
        },
        "date": 1792402991047,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "MemoryCacheBenchmarks.TryGetValue_MicrosoftExtensions - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079861"
          },
          {
            "name": "MemoryCacheBenchmarks.TryGetValue - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079861"
          },
          {
            "name": "MemoryCacheBenchmarks.Set_MicrosoftExtensions - allocated",
            "value": 104,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079861"
          },
          {
            "name": "MemoryCacheBenchmarks.Set - allocated",
            "value": 400,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079861"
          }
        ]
      }
    ],
    "libraries/Configuration - time": [
      {
        "commit": {
          "author": {
            "name": "Chase Crawford",
            "username": "dotnetcadet",
            "email": "56458944+dotnetcadet@users.noreply.github.com"
          },
          "committer": {
            "name": "GitHub",
            "username": "web-flow",
            "email": "noreply@github.com"
          },
          "id": "3861cd040532be00d816c57a1fc14b242b9195a4",
          "message": "Merge pull request #4 from assimalign/fix/benchmarks-filter-glob\n\nfix(benchmarks): stop PowerShell globbing the filter on Linux; fail runs that measure nothing",
          "timestamp": "2026-10-19T09:36:35Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/3861cd040532be00d816c57a1fc14b242b9195a4"
        },
        "date": 1792403002559,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "ConfigurationBindingBenchmarks.Bind_MicrosoftExtensions",
            "value": 14281.674,
            "range": "± 110.731",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079862\nmean 14291.213 ns"
          },
          {
            "name": "ConfigurationBindingBenchmarks.Bind",
            "value": 22427.873,
            "range": "± 194.36",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079862\nmean 22447.558 ns"
          },
          {
            "name": "ConfigurationLookupBenchmarks.GetValue_MicrosoftExtensions",
            "value": 32.225,
            "range": "± 0.038",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079862\nmean 32.243 ns"
          },
          {
            "name": "ConfigurationLookupBenchmarks.GetValue",
            "value": 460.752,
            "range": "± 5.542",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079862\nmean 462.64 ns"
          },
          {
            "name": "JsonConfigurationBenchmarks.Build_MicrosoftExtensions",
            "value": 6203.983,
            "range": "± 74.609",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079862\nmean 6215.527 ns"
          },
          {
            "name": "JsonConfigurationBenchmarks.Build",
            "value": 15841.886,
            "range": "± 90.681",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079862\nmean 15860.017 ns"
          },
          {
            "name": "PathBenchmarks.Parse(value=/Serv(...)\\0:Url [23])",
            "value": 324.753,
            "range": "± 4.124",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079862\nmean 326.084 ns"
          },
          {
            "name": "PathBenchmarks.Parse(value=Servi(...)quest [24])",
            "value": 332.128,
            "range": "± 6.598",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079862\nmean 333.105 ns"
          }
        ]
      }
    ],
    "libraries/Configuration - allocations": [
      {
        "commit": {
          "author": {
            "name": "Chase Crawford",
            "username": "dotnetcadet",
            "email": "56458944+dotnetcadet@users.noreply.github.com"
          },
          "committer": {
            "name": "GitHub",
            "username": "web-flow",
            "email": "noreply@github.com"
          },
          "id": "3861cd040532be00d816c57a1fc14b242b9195a4",
          "message": "Merge pull request #4 from assimalign/fix/benchmarks-filter-glob\n\nfix(benchmarks): stop PowerShell globbing the filter on Linux; fail runs that measure nothing",
          "timestamp": "2026-10-19T09:36:35Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/3861cd040532be00d816c57a1fc14b242b9195a4"
        },
        "date": 1792403004940,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "ConfigurationBindingBenchmarks.Bind_MicrosoftExtensions - allocated",
            "value": 11490,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079862"
          },
          {
            "name": "ConfigurationBindingBenchmarks.Bind - allocated",
            "value": 18212,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079862"
          },
          {
            "name": "ConfigurationLookupBenchmarks.GetValue_MicrosoftExtensions - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079862"
          },
          {
            "name": "ConfigurationLookupBenchmarks.GetValue - allocated",
            "value": 1088,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079862"
          },
          {
            "name": "JsonConfigurationBenchmarks.Build_MicrosoftExtensions - allocated",
            "value": 12536,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079862"
          },
          {
            "name": "JsonConfigurationBenchmarks.Build - allocated",
            "value": 26337,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079862"
          },
          {
            "name": "PathBenchmarks.Parse(value=/Serv(...)\\0:Url [23]) - allocated",
            "value": 1000,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079862"
          },
          {
            "name": "PathBenchmarks.Parse(value=Servi(...)quest [24]) - allocated",
            "value": 1000,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079862"
          }
        ]
      }
    ],
    "libraries/Connections - time": [
      {
        "commit": {
          "author": {
            "name": "Chase Crawford",
            "username": "dotnetcadet",
            "email": "56458944+dotnetcadet@users.noreply.github.com"
          },
          "committer": {
            "name": "GitHub",
            "username": "web-flow",
            "email": "noreply@github.com"
          },
          "id": "3861cd040532be00d816c57a1fc14b242b9195a4",
          "message": "Merge pull request #4 from assimalign/fix/benchmarks-filter-glob\n\nfix(benchmarks): stop PowerShell globbing the filter on Linux; fail runs that measure nothing",
          "timestamp": "2026-10-19T09:36:35Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/3861cd040532be00d816c57a1fc14b242b9195a4"
        },
        "date": 1792403020206,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "InMemoryConnectionBenchmarks.RoundTrip_SystemIOPipelines(PayloadSize=64)",
            "value": 417.769,
            "range": "± 0.982",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079863\nmean 417.939 ns"
          },
          {
            "name": "InMemoryConnectionBenchmarks.RoundTrip(PayloadSize=64)",
            "value": 461.678,
            "range": "± 1.358",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079863\nmean 461.968 ns"
          },
          {
            "name": "InMemoryConnectionBenchmarks.RoundTrip_SystemIOPipelines(PayloadSize=4096)",
            "value": 571.682,
            "range": "± 0.57",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079863\nmean 571.563 ns"
          },
          {
            "name": "InMemoryConnectionBenchmarks.RoundTrip(PayloadSize=4096)",
            "value": 697.849,
            "range": "± 1.078",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079863\nmean 697.674 ns"
          },
          {
            "name": "InMemoryConnectionListenerBenchmarks.ConnectAccept",
            "value": 844.234,
            "range": "± 5.927",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079863\nmean 843.824 ns"
          }
        ]
      }
    ]
  }
}