window.BENCHMARK_DATA = {
  "lastUpdate": 1792403226820,
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
    ],
    "libraries/Connections - allocations": [
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
        "date": 1792403022375,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "InMemoryConnectionBenchmarks.RoundTrip_SystemIOPipelines(PayloadSize=64) - allocated",
            "value": 144,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079863"
          },
          {
            "name": "InMemoryConnectionBenchmarks.RoundTrip(PayloadSize=64) - allocated",
            "value": 144,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079863"
          },
          {
            "name": "InMemoryConnectionBenchmarks.RoundTrip_SystemIOPipelines(PayloadSize=4096) - allocated",
            "value": 144,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079863"
          },
          {
            "name": "InMemoryConnectionBenchmarks.RoundTrip(PayloadSize=4096) - allocated",
            "value": 144,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079863"
          },
          {
            "name": "InMemoryConnectionListenerBenchmarks.ConnectAccept - allocated",
            "value": 1848,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079863"
          }
        ]
      }
    ],
    "libraries/Content - time": [
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
        "date": 1792403037409,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "BmffDocumentBenchmarks.Load",
            "value": 14026.116,
            "range": "± 63.318",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079864\nmean 14051.735 ns"
          },
          {
            "name": "MarkdownBenchmarks.Parse_Markdig",
            "value": 18260.311,
            "range": "± 110.086",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079864\nmean 18266.576 ns"
          },
          {
            "name": "MarkdownBenchmarks.Parse",
            "value": 56267.878,
            "range": "± 158.417",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079864\nmean 56281.499 ns"
          },
          {
            "name": "MarkdownBenchmarks.ToHtml_Markdig",
            "value": 22058.871,
            "range": "± 155.216",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079864\nmean 22035.842 ns"
          },
          {
            "name": "MarkdownBenchmarks.ToHtml",
            "value": 64109.529,
            "range": "± 236.858",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079864\nmean 64166.702 ns"
          },
          {
            "name": "YamlBenchmarks.Parse_YamlDotNet",
            "value": 49838.241,
            "range": "± 140.091",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079864\nmean 49834.334 ns"
          },
          {
            "name": "YamlBenchmarks.Parse",
            "value": 18362.476,
            "range": "± 75.779",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079864\nmean 18370.844 ns"
          }
        ]
      }
    ],
    "libraries/Content - allocations": [
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
        "date": 1792403040033,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "BmffDocumentBenchmarks.Load - allocated",
            "value": 19168,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079864"
          },
          {
            "name": "MarkdownBenchmarks.Parse_Markdig - allocated",
            "value": 19984,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079864"
          },
          {
            "name": "MarkdownBenchmarks.Parse - allocated",
            "value": 127656,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079864"
          },
          {
            "name": "MarkdownBenchmarks.ToHtml_Markdig - allocated",
            "value": 24224,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079864"
          },
          {
            "name": "MarkdownBenchmarks.ToHtml - allocated",
            "value": 147665,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079864"
          },
          {
            "name": "YamlBenchmarks.Parse_YamlDotNet - allocated",
            "value": 55928,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079864"
          },
          {
            "name": "YamlBenchmarks.Parse - allocated",
            "value": 46072,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079864"
          }
        ]
      }
    ],
    "libraries/Core - time": [
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
        "date": 1792403054175,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "FileSystemPathBenchmarks.GetSegments",
            "value": 166.343,
            "range": "± 1.216",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079866\nmean 166.503 ns"
          },
          {
            "name": "FileSystemPathBenchmarks.Parse(value=//tes(...)e.txt [33])",
            "value": 2621.632,
            "range": "± 25.735",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079866\nmean 2622.586 ns"
          },
          {
            "name": "FileSystemPathBenchmarks.Parse(value=C:/te(...)e.txt [34])",
            "value": 2827.688,
            "range": "± 11.012",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079866\nmean 2831.062 ns"
          },
          {
            "name": "GlobBenchmarks.IsMatch_Regex(Pattern=p?th/(...)].txt [21])",
            "value": 1871.198,
            "range": "± 2.555",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079866\nmean 1870.977 ns"
          },
          {
            "name": "GlobBenchmarks.IsMatch(Pattern=p?th/(...)].txt [21])",
            "value": 5887.252,
            "range": "± 25.951",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079866\nmean 5883.783 ns"
          },
          {
            "name": "GlobBenchmarks.IsMatch_Regex(Pattern=src/**/*.{cs,json})",
            "value": 3314.945,
            "range": "± 20.004",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079866\nmean 3325.194 ns"
          },
          {
            "name": "GlobBenchmarks.IsMatch(Pattern=src/**/*.{cs,json})",
            "value": 11655.737,
            "range": "± 45.122",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079866\nmean 11676.092 ns"
          }
        ]
      }
    ],
    "libraries/Core - allocations": [
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
        "date": 1792403056036,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "FileSystemPathBenchmarks.GetSegments - allocated",
            "value": 520,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079866"
          },
          {
            "name": "FileSystemPathBenchmarks.Parse(value=//tes(...)e.txt [33]) - allocated",
            "value": 3904,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079866"
          },
          {
            "name": "FileSystemPathBenchmarks.Parse(value=C:/te(...)e.txt [34]) - allocated",
            "value": 4272,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079866"
          },
          {
            "name": "GlobBenchmarks.IsMatch_Regex(Pattern=p?th/(...)].txt [21]) - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079866"
          },
          {
            "name": "GlobBenchmarks.IsMatch(Pattern=p?th/(...)].txt [21]) - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079866"
          },
          {
            "name": "GlobBenchmarks.IsMatch_Regex(Pattern=src/**/*.{cs,json}) - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079866"
          },
          {
            "name": "GlobBenchmarks.IsMatch(Pattern=src/**/*.{cs,json}) - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079866"
          }
        ]
      }
    ],
    "libraries/DependencyInjection - time": [
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
        "date": 1792403071453,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "ServiceProviderBuildBenchmarks.BuildAndResolve_MicrosoftExtensions",
            "value": 3050.401,
            "range": "± 40.244",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079867\nmean 3044.901 ns"
          },
          {
            "name": "ServiceProviderBuildBenchmarks.BuildAndResolve",
            "value": 2527.962,
            "range": "± 29.487",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079867\nmean 2535.089 ns"
          },
          {
            "name": "ServiceResolutionBenchmarks.CreateScopeAndResolve_MicrosoftExtensions",
            "value": 131.979,
            "range": "± 2.349",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079867\nmean 132.339 ns"
          },
          {
            "name": "ServiceResolutionBenchmarks.CreateScopeAndResolve",
            "value": 153.729,
            "range": "± 3.028",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079867\nmean 153.724 ns"
          },
          {
            "name": "ServiceResolutionBenchmarks.ResolveSingleton_MicrosoftExtensions",
            "value": 13.765,
            "range": "± 0.011",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079867\nmean 13.767 ns"
          },
          {
            "name": "ServiceResolutionBenchmarks.ResolveSingleton",
            "value": 19.553,
            "range": "± 0.045",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079867\nmean 19.551 ns"
          },
          {
            "name": "ServiceResolutionBenchmarks.ResolveTransient_MicrosoftExtensions",
            "value": 67.786,
            "range": "± 0.167",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079867\nmean 67.815 ns"
          },
          {
            "name": "ServiceResolutionBenchmarks.ResolveTransient",
            "value": 62.616,
            "range": "± 0.149",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079867\nmean 62.616 ns"
          }
        ]
      }
    ],
    "libraries/DependencyInjection - allocations": [
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
        "date": 1792403074059,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "ServiceProviderBuildBenchmarks.BuildAndResolve_MicrosoftExtensions - allocated",
            "value": 6296,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079867"
          },
          {
            "name": "ServiceProviderBuildBenchmarks.BuildAndResolve - allocated",
            "value": 5960,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079867"
          },
          {
            "name": "ServiceResolutionBenchmarks.CreateScopeAndResolve_MicrosoftExtensions - allocated",
            "value": 368,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079867"
          },
          {
            "name": "ServiceResolutionBenchmarks.CreateScopeAndResolve - allocated",
            "value": 344,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079867"
          },
          {
            "name": "ServiceResolutionBenchmarks.ResolveSingleton_MicrosoftExtensions - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079867"
          },
          {
            "name": "ServiceResolutionBenchmarks.ResolveSingleton - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079867"
          },
          {
            "name": "ServiceResolutionBenchmarks.ResolveTransient_MicrosoftExtensions - allocated",
            "value": 32,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079867"
          },
          {
            "name": "ServiceResolutionBenchmarks.ResolveTransient - allocated",
            "value": 32,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079867"
          }
        ]
      }
    ],
    "libraries/Dns - time": [
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
        "date": 1792403085441,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "DnsMessageBenchmarks.WriteQuery",
            "value": 495.014,
            "range": "± 7.151",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079869\nmean 496.24 ns"
          },
          {
            "name": "DnsMessageBenchmarks.ParseResponse",
            "value": 1237.463,
            "range": "± 20.334",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079869\nmean 1239.453 ns"
          },
          {
            "name": "DnsMessageBenchmarks.WriteResponse",
            "value": 2844.939,
            "range": "± 38.043",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079869\nmean 2852.08 ns"
          }
        ]
      }
    ],
    "libraries/Dns - allocations": [
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
        "date": 1792403087531,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "DnsMessageBenchmarks.WriteQuery - allocated",
            "value": 848,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079869"
          },
          {
            "name": "DnsMessageBenchmarks.ParseResponse - allocated",
            "value": 2632,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079869"
          },
          {
            "name": "DnsMessageBenchmarks.WriteResponse - allocated",
            "value": 5520,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079869"
          }
        ]
      }
    ],
    "libraries/FileSystem - time": [
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
        "date": 1792403106491,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "GlobMatcherBenchmarks.Filter_MicrosoftExtensions",
            "value": 133390.836,
            "range": "± 506.694",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079870\nmean 133508.084 ns"
          },
          {
            "name": "GlobMatcherBenchmarks.Filter",
            "value": 54850.034,
            "range": "± 66.586",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079870\nmean 54864.848 ns"
          },
          {
            "name": "GlobMatcherBenchmarks.Filter_FromStrings",
            "value": 675690.939,
            "range": "± 2525.407",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079870\nmean 676439.296 ns"
          },
          {
            "name": "InMemoryFileSystemBenchmarks.ReadFile",
            "value": 23744.916,
            "range": "± 76.512",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079870\nmean 23741.909 ns"
          },
          {
            "name": "PhysicalFileSystemBenchmarks.ReadFile_SystemIO",
            "value": 6740.189,
            "range": "± 10.675",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079870\nmean 6736.345 ns"
          },
          {
            "name": "PhysicalFileSystemBenchmarks.ReadFile",
            "value": 17307.026,
            "range": "± 66.694",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079870\nmean 17299.117 ns"
          }
        ]
      }
    ],
    "libraries/FileSystem - allocations": [
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
        "date": 1792403109233,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "GlobMatcherBenchmarks.Filter_MicrosoftExtensions - allocated",
            "value": 186416,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079870"
          },
          {
            "name": "GlobMatcherBenchmarks.Filter - allocated",
            "value": 13760,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079870"
          },
          {
            "name": "GlobMatcherBenchmarks.Filter_FromStrings - allocated",
            "value": 883140,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079870"
          },
          {
            "name": "InMemoryFileSystemBenchmarks.ReadFile - allocated",
            "value": 23872,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079870"
          },
          {
            "name": "PhysicalFileSystemBenchmarks.ReadFile_SystemIO - allocated",
            "value": 232,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079870"
          },
          {
            "name": "PhysicalFileSystemBenchmarks.ReadFile - allocated",
            "value": 8936,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079870"
          }
        ]
      }
    ],
    "libraries/Hosting - time": [
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
        "date": 1792403123730,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "HostLifecycleBenchmarks.BuildStartStop_MicrosoftExtensions(ServiceCount=1)",
            "value": 230310.441,
            "range": "± 16816.68",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079872\nmean 223263.847 ns"
          },
          {
            "name": "HostLifecycleBenchmarks.BuildStartStop(ServiceCount=1)",
            "value": 2104.405,
            "range": "± 12.966",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079872\nmean 2104.912 ns"
          },
          {
            "name": "HostLifecycleBenchmarks.BuildStartStop_MicrosoftExtensions(ServiceCount=16)",
            "value": 236766.799,
            "range": "± 3362.789",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079872\nmean 237052.64 ns"
          },
          {
            "name": "HostLifecycleBenchmarks.BuildStartStop(ServiceCount=16)",
            "value": 5646.147,
            "range": "± 30.003",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079872\nmean 5649.784 ns"
          }
        ]
      }
    ],
    "libraries/Hosting - allocations": [
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
        "date": 1792403125963,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "HostLifecycleBenchmarks.BuildStartStop_MicrosoftExtensions(ServiceCount=1) - allocated",
            "value": 74494,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079872"
          },
          {
            "name": "HostLifecycleBenchmarks.BuildStartStop(ServiceCount=1) - allocated",
            "value": 2336,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079872"
          },
          {
            "name": "HostLifecycleBenchmarks.BuildStartStop_MicrosoftExtensions(ServiceCount=16) - allocated",
            "value": 96503,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079872"
          },
          {
            "name": "HostLifecycleBenchmarks.BuildStartStop(ServiceCount=16) - allocated",
            "value": 9056,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079872"
          }
        ]
      }
    ],
    "libraries/Http - time": [
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
        "date": 1792403137991,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "Http1RequestBenchmarks.ServeRequest(Header=bearer)",
            "value": 2540.929,
            "range": "± 16.871",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079875\nmean 2542.987 ns"
          },
          {
            "name": "Http1RequestBenchmarks.ServeRequest(Header=cookie)",
            "value": 2569.958,
            "range": "± 18.393",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079875\nmean 2571.935 ns"
          },
          {
            "name": "Http1RequestBenchmarks.ServeRequest(Header=none)",
            "value": 2178.281,
            "range": "± 14.833",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079875\nmean 2177.98 ns"
          },
          {
            "name": "HttpCookieBenchmarks.FormatSetCookie_AspNetCore",
            "value": 99.754,
            "range": "± 0.51",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079875\nmean 99.756 ns"
          },
          {
            "name": "HttpCookieBenchmarks.FormatSetCookie",
            "value": 160.342,
            "range": "± 0.649",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079875\nmean 160.265 ns"
          },
          {
            "name": "HttpCookieBenchmarks.ParseCookieHeader_AspNetCore",
            "value": 609.851,
            "range": "± 2.538",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079875\nmean 610.419 ns"
          },
          {
            "name": "HttpCookieBenchmarks.ParseCookieHeader",
            "value": 512.466,
            "range": "± 3.303",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079875\nmean 513.255 ns"
          },
          {
            "name": "HttpAcceptParserBenchmarks.ParseAccept_AspNetCore",
            "value": 1304.567,
            "range": "± 9.57",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079875\nmean 1304.848 ns"
          },
          {
            "name": "HttpAcceptParserBenchmarks.ParseAccept",
            "value": 1183.238,
            "range": "± 6.499",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079875\nmean 1184.938 ns"
          },
          {
            "name": "HttpForwardedBenchmarks.ParseForwarded",
            "value": 1066.539,
            "range": "± 7.226",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079875\nmean 1067.577 ns"
          },
          {
            "name": "HttpForwardedBenchmarks.ParseXForwardedFor",
            "value": 336.448,
            "range": "± 0.997",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079875\nmean 336.575 ns"
          }
        ]
      }
    ],
    "libraries/Http - allocations": [
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
        "date": 1792403140692,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "Http1RequestBenchmarks.ServeRequest(Header=bearer) - allocated",
            "value": 4776,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079875"
          },
          {
            "name": "Http1RequestBenchmarks.ServeRequest(Header=cookie) - allocated",
            "value": 5496,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079875"
          },
          {
            "name": "Http1RequestBenchmarks.ServeRequest(Header=none) - allocated",
            "value": 3272,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079875"
          },
          {
            "name": "HttpCookieBenchmarks.FormatSetCookie_AspNetCore - allocated",
            "value": 360,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079875"
          },
          {
            "name": "HttpCookieBenchmarks.FormatSetCookie - allocated",
            "value": 1040,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079875"
          },
          {
            "name": "HttpCookieBenchmarks.ParseCookieHeader_AspNetCore - allocated",
            "value": 496,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079875"
          },
          {
            "name": "HttpCookieBenchmarks.ParseCookieHeader - allocated",
            "value": 2176,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079875"
          },
          {
            "name": "HttpAcceptParserBenchmarks.ParseAccept_AspNetCore - allocated",
            "value": 1672,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079875"
          },
          {
            "name": "HttpAcceptParserBenchmarks.ParseAccept - allocated",
            "value": 2384,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079875"
          },
          {
            "name": "HttpForwardedBenchmarks.ParseForwarded - allocated",
            "value": 2144,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079875"
          },
          {
            "name": "HttpForwardedBenchmarks.ParseXForwardedFor - allocated",
            "value": 696,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079875"
          }
        ]
      }
    ],
    "libraries/IdentityModel - time": [
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
        "date": 1792403156063,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "JsonWebTokenBenchmarks.Parse_MicrosoftIdentityModel",
            "value": 1581.314,
            "range": "± 19.422",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079877\nmean 1586.571 ns"
          },
          {
            "name": "JsonWebTokenBenchmarks.Parse",
            "value": 2881.514,
            "range": "± 17.554",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079877\nmean 2883.585 ns"
          },
          {
            "name": "JsonWebTokenBenchmarks.Validate_MicrosoftIdentityModel",
            "value": 84685.615,
            "range": "± 66.771",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079877\nmean 84698.574 ns"
          },
          {
            "name": "JsonWebTokenBenchmarks.Validate",
            "value": 256067.252,
            "range": "± 321.582",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079877\nmean 256167.91 ns"
          },
          {
            "name": "JsonWebTokenBenchmarks.Write_MicrosoftIdentityModel",
            "value": 30767.955,
            "range": "± 24.124",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079877\nmean 30771.248 ns"
          },
          {
            "name": "JsonWebTokenBenchmarks.Write",
            "value": 31104.44,
            "range": "± 37.522",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079877\nmean 31104.908 ns"
          }
        ]
      }
    ],
    "libraries/IdentityModel - allocations": [
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
        "date": 1792403158269,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "JsonWebTokenBenchmarks.Parse_MicrosoftIdentityModel - allocated",
            "value": 2520,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079877"
          },
          {
            "name": "JsonWebTokenBenchmarks.Parse - allocated",
            "value": 5784,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079877"
          },
          {
            "name": "JsonWebTokenBenchmarks.Validate_MicrosoftIdentityModel - allocated",
            "value": 5073,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079877"
          },
          {
            "name": "JsonWebTokenBenchmarks.Validate - allocated",
            "value": 10554,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079877"
          },
          {
            "name": "JsonWebTokenBenchmarks.Write_MicrosoftIdentityModel - allocated",
            "value": 2056,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079877"
          },
          {
            "name": "JsonWebTokenBenchmarks.Write - allocated",
            "value": 5032,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079877"
          }
        ]
      }
    ],
    "libraries/Logging - time": [
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
        "date": 1792403173894,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "LoggerBenchmarks.LogDisabled_MicrosoftExtensions",
            "value": 23.386,
            "range": "± 0.247",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079879\nmean 23.506 ns"
          },
          {
            "name": "LoggerBenchmarks.LogDisabled",
            "value": 29.246,
            "range": "± 0.566",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079879\nmean 29.174 ns"
          },
          {
            "name": "LoggerBenchmarks.LogEnabled_MicrosoftExtensions",
            "value": 46.904,
            "range": "± 0.197",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079879\nmean 46.914 ns"
          },
          {
            "name": "LoggerBenchmarks.LogEnabled",
            "value": 162.824,
            "range": "± 1.511",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079879\nmean 162.752 ns"
          },
          {
            "name": "LoggerBenchmarks.Scope_MicrosoftExtensions",
            "value": 38.379,
            "range": "± 0.085",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079879\nmean 38.384 ns"
          },
          {
            "name": "LoggerBenchmarks.Scope",
            "value": 279.416,
            "range": "± 2.898",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079879\nmean 280.909 ns"
          }
        ]
      }
    ],
    "libraries/Logging - allocations": [
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
        "date": 1792403176228,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "LoggerBenchmarks.LogDisabled_MicrosoftExtensions - allocated",
            "value": 32,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079879"
          },
          {
            "name": "LoggerBenchmarks.LogDisabled - allocated",
            "value": 216,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079879"
          },
          {
            "name": "LoggerBenchmarks.LogEnabled_MicrosoftExtensions - allocated",
            "value": 88,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079879"
          },
          {
            "name": "LoggerBenchmarks.LogEnabled - allocated",
            "value": 368,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079879"
          },
          {
            "name": "LoggerBenchmarks.Scope_MicrosoftExtensions - allocated",
            "value": 24,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079879"
          },
          {
            "name": "LoggerBenchmarks.Scope - allocated",
            "value": 456,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079879"
          }
        ]
      }
    ],
    "libraries/ObjectMapping - time": [
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
        "date": 1792403192456,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "MapperBenchmarks.MapFlat_AutoMapper",
            "value": 49.241,
            "range": "± 0.133",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079882\nmean 49.281 ns"
          },
          {
            "name": "MapperBenchmarks.MapFlat",
            "value": 108.029,
            "range": "± 0.279",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079882\nmean 108.056 ns"
          },
          {
            "name": "MapperBenchmarks.MapNested_AutoMapper",
            "value": 311.134,
            "range": "± 2.215",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079882\nmean 310.791 ns"
          },
          {
            "name": "MapperBenchmarks.MapNested",
            "value": 1883.809,
            "range": "± 3.676",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079882\nmean 1884.224 ns"
          }
        ]
      }
    ],
    "libraries/ObjectMapping - allocations": [
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
        "date": 1792403195145,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "MapperBenchmarks.MapFlat_AutoMapper - allocated",
            "value": 56,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079882"
          },
          {
            "name": "MapperBenchmarks.MapFlat - allocated",
            "value": 144,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079882"
          },
          {
            "name": "MapperBenchmarks.MapNested_AutoMapper - allocated",
            "value": 952,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079882"
          },
          {
            "name": "MapperBenchmarks.MapNested - allocated",
            "value": 2288,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079882"
          }
        ]
      }
    ],
    "libraries/ObjectPool - time": [
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
        "date": 1792403210958,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "DefaultObjectPoolBenchmarks.RentReturn_MicrosoftExtensions",
            "value": 11.561,
            "range": "± 0.052",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079884\nmean 11.577 ns"
          },
          {
            "name": "DefaultObjectPoolBenchmarks.RentReturn",
            "value": 11.468,
            "range": "± 0.104",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079884\nmean 11.463 ns"
          },
          {
            "name": "DefaultObjectPoolBenchmarks.RentTwoReturnTwo",
            "value": 28.167,
            "range": "± 0.092",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079884\nmean 28.131 ns"
          }
        ]
      }
    ],
    "libraries/ObjectPool - allocations": [
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
        "date": 1792403213288,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "DefaultObjectPoolBenchmarks.RentReturn_MicrosoftExtensions - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079884"
          },
          {
            "name": "DefaultObjectPoolBenchmarks.RentReturn - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079884"
          },
          {
            "name": "DefaultObjectPoolBenchmarks.RentTwoReturnTwo - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079884"
          }
        ]
      }
    ],
    "libraries/ObjectValidation - time": [
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
        "date": 1792403226454,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "ValidatorBenchmarks.ValidateComparisons_FluentValidation",
            "value": 5713.975,
            "range": "± 88.954",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079886\nmean 5730.851 ns"
          },
          {
            "name": "ValidatorBenchmarks.ValidateComparisons",
            "value": 3645.515,
            "range": "± 32.097",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079886\nmean 3648.519 ns"
          },
          {
            "name": "ValidatorBenchmarks.ValidateInvalid_FluentValidation",
            "value": 5306.654,
            "range": "± 13.863",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079886\nmean 5307.863 ns"
          },
          {
            "name": "ValidatorBenchmarks.ValidateInvalid",
            "value": 3961.561,
            "range": "± 33.058",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079886\nmean 3966.113 ns"
          },
          {
            "name": "ValidatorBenchmarks.Validate_FluentValidation",
            "value": 1525.264,
            "range": "± 13.083",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079886\nmean 1525.37 ns"
          },
          {
            "name": "ValidatorBenchmarks.Validate",
            "value": 4317.606,
            "range": "± 13.22",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079886\nmean 4316.205 ns"
          }
        ]
      }
    ]
  }
}