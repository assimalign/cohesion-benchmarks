window.BENCHMARK_DATA = {
  "lastUpdate": 1792448931317,
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
      },
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
          "id": "00e8cb49c3ca66a73eac2bf997d449bd83a7801a",
          "message": "Merge pull request #5 from assimalign/fix/benchmarks-ci-first-run\n\nfix(benchmarks): first measuring run failures (history bootstrap, keep-alive close, Python stall)",
          "timestamp": "2026-10-19T10:03:39Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/00e8cb49c3ca66a73eac2bf997d449bd83a7801a"
        },
        "date": 1792416625329,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "GenericPlannerBenchmarks.CreatePlan",
            "value": 962.517,
            "range": "± 10.659",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080031\nmean 959.815 ns"
          },
          {
            "name": "GenericPlannerBenchmarks.ValidatePlan",
            "value": 4313.43,
            "range": "± 101.742",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080031\nmean 4332.774 ns"
          },
          {
            "name": "ResourceManifestBenchmarks.Parse",
            "value": 6319.882,
            "range": "± 42.878",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080031\nmean 6325.579 ns"
          },
          {
            "name": "ResourceManifestBenchmarks.ComputeHash",
            "value": 19591.149,
            "range": "± 91.703",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080031\nmean 19574.716 ns"
          }
        ]
      },
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
          "id": "06c9dcd86539ac3365bc0bb6b3432db5ad769ce5",
          "message": "Merge pull request #7 from assimalign/fix/record-both-metrics-on-alert\n\nRecord allocations even when the time series alerts",
          "timestamp": "2026-10-19T20:26:47Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/06c9dcd86539ac3365bc0bb6b3432db5ad769ce5"
        },
        "date": 1792448652465,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "GenericPlannerBenchmarks.CreatePlan",
            "value": 1154.966,
            "range": "± 23.549",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080317\nmean 1159.809 ns"
          },
          {
            "name": "GenericPlannerBenchmarks.ValidatePlan",
            "value": 5366.255,
            "range": "± 74.92",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080317\nmean 5378.395 ns"
          },
          {
            "name": "ResourceManifestBenchmarks.Parse",
            "value": 8199.22,
            "range": "± 18.151",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080317\nmean 8195.677 ns"
          },
          {
            "name": "ResourceManifestBenchmarks.ComputeHash",
            "value": 26459.04,
            "range": "± 191.437",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080317\nmean 26474.466 ns"
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
      },
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
          "id": "00e8cb49c3ca66a73eac2bf997d449bd83a7801a",
          "message": "Merge pull request #5 from assimalign/fix/benchmarks-ci-first-run\n\nfix(benchmarks): first measuring run failures (history bootstrap, keep-alive close, Python stall)",
          "timestamp": "2026-10-19T10:03:39Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/00e8cb49c3ca66a73eac2bf997d449bd83a7801a"
        },
        "date": 1792416628136,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "GenericPlannerBenchmarks.CreatePlan - allocated",
            "value": 3512,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080031"
          },
          {
            "name": "GenericPlannerBenchmarks.ValidatePlan - allocated",
            "value": 14353,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080031"
          },
          {
            "name": "ResourceManifestBenchmarks.Parse - allocated",
            "value": 5208,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080031"
          },
          {
            "name": "ResourceManifestBenchmarks.ComputeHash - allocated",
            "value": 33025,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080031"
          }
        ]
      },
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
          "id": "06c9dcd86539ac3365bc0bb6b3432db5ad769ce5",
          "message": "Merge pull request #7 from assimalign/fix/record-both-metrics-on-alert\n\nRecord allocations even when the time series alerts",
          "timestamp": "2026-10-19T20:26:47Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/06c9dcd86539ac3365bc0bb6b3432db5ad769ce5"
        },
        "date": 1792448655205,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "GenericPlannerBenchmarks.CreatePlan - allocated",
            "value": 3512,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080317"
          },
          {
            "name": "GenericPlannerBenchmarks.ValidatePlan - allocated",
            "value": 14353,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080317"
          },
          {
            "name": "ResourceManifestBenchmarks.Parse - allocated",
            "value": 5208,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080317"
          },
          {
            "name": "ResourceManifestBenchmarks.ComputeHash - allocated",
            "value": 33025,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080317"
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
      },
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
          "id": "00e8cb49c3ca66a73eac2bf997d449bd83a7801a",
          "message": "Merge pull request #5 from assimalign/fix/benchmarks-ci-first-run\n\nfix(benchmarks): first measuring run failures (history bootstrap, keep-alive close, Python stall)",
          "timestamp": "2026-10-19T10:03:39Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/00e8cb49c3ca66a73eac2bf997d449bd83a7801a"
        },
        "date": 1792416644168,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "MemoryCacheBenchmarks.TryGetValue_MicrosoftExtensions",
            "value": 41.477,
            "range": "± 0.052",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080032\nmean 41.482 ns"
          },
          {
            "name": "MemoryCacheBenchmarks.TryGetValue",
            "value": 77.876,
            "range": "± 0.066",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080032\nmean 77.895 ns"
          },
          {
            "name": "MemoryCacheBenchmarks.Set_MicrosoftExtensions",
            "value": 90.955,
            "range": "± 0.325",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080032\nmean 91.072 ns"
          },
          {
            "name": "MemoryCacheBenchmarks.Set",
            "value": 150.882,
            "range": "± 1.056",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080032\nmean 150.798 ns"
          }
        ]
      },
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
          "id": "06c9dcd86539ac3365bc0bb6b3432db5ad769ce5",
          "message": "Merge pull request #7 from assimalign/fix/record-both-metrics-on-alert\n\nRecord allocations even when the time series alerts",
          "timestamp": "2026-10-19T20:26:47Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/06c9dcd86539ac3365bc0bb6b3432db5ad769ce5"
        },
        "date": 1792448669356,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "MemoryCacheBenchmarks.TryGetValue_MicrosoftExtensions",
            "value": 41.889,
            "range": "± 0.098",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080321\nmean 41.926 ns"
          },
          {
            "name": "MemoryCacheBenchmarks.TryGetValue",
            "value": 77.847,
            "range": "± 0.062",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080321\nmean 77.866 ns"
          },
          {
            "name": "MemoryCacheBenchmarks.Set_MicrosoftExtensions",
            "value": 91.158,
            "range": "± 1.248",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080321\nmean 91.59 ns"
          },
          {
            "name": "MemoryCacheBenchmarks.Set",
            "value": 144.431,
            "range": "± 0.755",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080321\nmean 144.661 ns"
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
      },
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
          "id": "06c9dcd86539ac3365bc0bb6b3432db5ad769ce5",
          "message": "Merge pull request #7 from assimalign/fix/record-both-metrics-on-alert\n\nRecord allocations even when the time series alerts",
          "timestamp": "2026-10-19T20:26:47Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/06c9dcd86539ac3365bc0bb6b3432db5ad769ce5"
        },
        "date": 1792448672139,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "MemoryCacheBenchmarks.TryGetValue_MicrosoftExtensions - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080321"
          },
          {
            "name": "MemoryCacheBenchmarks.TryGetValue - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080321"
          },
          {
            "name": "MemoryCacheBenchmarks.Set_MicrosoftExtensions - allocated",
            "value": 104,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080321"
          },
          {
            "name": "MemoryCacheBenchmarks.Set - allocated",
            "value": 400,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080321"
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
      },
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
          "id": "00e8cb49c3ca66a73eac2bf997d449bd83a7801a",
          "message": "Merge pull request #5 from assimalign/fix/benchmarks-ci-first-run\n\nfix(benchmarks): first measuring run failures (history bootstrap, keep-alive close, Python stall)",
          "timestamp": "2026-10-19T10:03:39Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/00e8cb49c3ca66a73eac2bf997d449bd83a7801a"
        },
        "date": 1792416657989,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "ConfigurationBindingBenchmarks.Bind_MicrosoftExtensions",
            "value": 6540.256,
            "range": "± 167.132",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080034\nmean 6599.889 ns"
          },
          {
            "name": "ConfigurationBindingBenchmarks.Bind",
            "value": 12042.096,
            "range": "± 272.693",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080034\nmean 11981.121 ns"
          },
          {
            "name": "ConfigurationLookupBenchmarks.GetValue_MicrosoftExtensions",
            "value": 11.641,
            "range": "± 0.228",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080034\nmean 11.667 ns"
          },
          {
            "name": "ConfigurationLookupBenchmarks.GetValue",
            "value": 234.86,
            "range": "± 4.708",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080034\nmean 234.908 ns"
          },
          {
            "name": "JsonConfigurationBenchmarks.Build_MicrosoftExtensions",
            "value": 3019.718,
            "range": "± 62.665",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080034\nmean 3017.724 ns"
          },
          {
            "name": "JsonConfigurationBenchmarks.Build",
            "value": 8051.028,
            "range": "± 127.346",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080034\nmean 8080.301 ns"
          },
          {
            "name": "PathBenchmarks.Parse(value=/Serv(...)\\0:Url [23])",
            "value": 153.25,
            "range": "± 4.059",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080034\nmean 153.994 ns"
          },
          {
            "name": "PathBenchmarks.Parse(value=Servi(...)quest [24])",
            "value": 154.689,
            "range": "± 4.275",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080034\nmean 154.213 ns"
          }
        ]
      },
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
          "id": "06c9dcd86539ac3365bc0bb6b3432db5ad769ce5",
          "message": "Merge pull request #7 from assimalign/fix/record-both-metrics-on-alert\n\nRecord allocations even when the time series alerts",
          "timestamp": "2026-10-19T20:26:47Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/06c9dcd86539ac3365bc0bb6b3432db5ad769ce5"
        },
        "date": 1792448685196,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "ConfigurationBindingBenchmarks.Bind_MicrosoftExtensions",
            "value": 14237.929,
            "range": "± 67.725",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080325\nmean 14242.676 ns"
          },
          {
            "name": "ConfigurationBindingBenchmarks.Bind",
            "value": 21411.016,
            "range": "± 98.684",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080325\nmean 21423.672 ns"
          },
          {
            "name": "ConfigurationLookupBenchmarks.GetValue_MicrosoftExtensions",
            "value": 33.367,
            "range": "± 0.038",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080325\nmean 33.375 ns"
          },
          {
            "name": "ConfigurationLookupBenchmarks.GetValue",
            "value": 444.898,
            "range": "± 2.727",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080325\nmean 444.778 ns"
          },
          {
            "name": "JsonConfigurationBenchmarks.Build_MicrosoftExtensions",
            "value": 6062.7,
            "range": "± 24.745",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080325\nmean 6061.352 ns"
          },
          {
            "name": "JsonConfigurationBenchmarks.Build",
            "value": 15131.142,
            "range": "± 69.411",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080325\nmean 15132.076 ns"
          },
          {
            "name": "PathBenchmarks.Parse(value=/Serv(...)\\0:Url [23])",
            "value": 310.839,
            "range": "± 2.524",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080325\nmean 310.94 ns"
          },
          {
            "name": "PathBenchmarks.Parse(value=Servi(...)quest [24])",
            "value": 306.899,
            "range": "± 2.9",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080325\nmean 306.468 ns"
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
      },
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
          "id": "00e8cb49c3ca66a73eac2bf997d449bd83a7801a",
          "message": "Merge pull request #5 from assimalign/fix/benchmarks-ci-first-run\n\nfix(benchmarks): first measuring run failures (history bootstrap, keep-alive close, Python stall)",
          "timestamp": "2026-10-19T10:03:39Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/00e8cb49c3ca66a73eac2bf997d449bd83a7801a"
        },
        "date": 1792416660075,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "ConfigurationBindingBenchmarks.Bind_MicrosoftExtensions - allocated",
            "value": 11490,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080034"
          },
          {
            "name": "ConfigurationBindingBenchmarks.Bind - allocated",
            "value": 18211,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080034"
          },
          {
            "name": "ConfigurationLookupBenchmarks.GetValue_MicrosoftExtensions - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080034"
          },
          {
            "name": "ConfigurationLookupBenchmarks.GetValue - allocated",
            "value": 1088,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080034"
          },
          {
            "name": "JsonConfigurationBenchmarks.Build_MicrosoftExtensions - allocated",
            "value": 12536,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080034"
          },
          {
            "name": "JsonConfigurationBenchmarks.Build - allocated",
            "value": 26337,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080034"
          },
          {
            "name": "PathBenchmarks.Parse(value=/Serv(...)\\0:Url [23]) - allocated",
            "value": 1000,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080034"
          },
          {
            "name": "PathBenchmarks.Parse(value=Servi(...)quest [24]) - allocated",
            "value": 1000,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080034"
          }
        ]
      },
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
          "id": "06c9dcd86539ac3365bc0bb6b3432db5ad769ce5",
          "message": "Merge pull request #7 from assimalign/fix/record-both-metrics-on-alert\n\nRecord allocations even when the time series alerts",
          "timestamp": "2026-10-19T20:26:47Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/06c9dcd86539ac3365bc0bb6b3432db5ad769ce5"
        },
        "date": 1792448688664,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "ConfigurationBindingBenchmarks.Bind_MicrosoftExtensions - allocated",
            "value": 11490,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080325"
          },
          {
            "name": "ConfigurationBindingBenchmarks.Bind - allocated",
            "value": 18212,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080325"
          },
          {
            "name": "ConfigurationLookupBenchmarks.GetValue_MicrosoftExtensions - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080325"
          },
          {
            "name": "ConfigurationLookupBenchmarks.GetValue - allocated",
            "value": 1088,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080325"
          },
          {
            "name": "JsonConfigurationBenchmarks.Build_MicrosoftExtensions - allocated",
            "value": 12536,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080325"
          },
          {
            "name": "JsonConfigurationBenchmarks.Build - allocated",
            "value": 26337,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080325"
          },
          {
            "name": "PathBenchmarks.Parse(value=/Serv(...)\\0:Url [23]) - allocated",
            "value": 1000,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080325"
          },
          {
            "name": "PathBenchmarks.Parse(value=Servi(...)quest [24]) - allocated",
            "value": 1000,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080325"
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
      },
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
          "id": "00e8cb49c3ca66a73eac2bf997d449bd83a7801a",
          "message": "Merge pull request #5 from assimalign/fix/benchmarks-ci-first-run\n\nfix(benchmarks): first measuring run failures (history bootstrap, keep-alive close, Python stall)",
          "timestamp": "2026-10-19T10:03:39Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/00e8cb49c3ca66a73eac2bf997d449bd83a7801a"
        },
        "date": 1792416674860,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "InMemoryConnectionBenchmarks.RoundTrip_SystemIOPipelines(PayloadSize=64)",
            "value": 417.276,
            "range": "± 0.554",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080035\nmean 417.4 ns"
          },
          {
            "name": "InMemoryConnectionBenchmarks.RoundTrip(PayloadSize=64)",
            "value": 471.608,
            "range": "± 0.57",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080035\nmean 471.539 ns"
          },
          {
            "name": "InMemoryConnectionBenchmarks.RoundTrip_SystemIOPipelines(PayloadSize=4096)",
            "value": 591.485,
            "range": "± 0.837",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080035\nmean 591.7 ns"
          },
          {
            "name": "InMemoryConnectionBenchmarks.RoundTrip(PayloadSize=4096)",
            "value": 603.295,
            "range": "± 0.611",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080035\nmean 603.307 ns"
          },
          {
            "name": "InMemoryConnectionListenerBenchmarks.ConnectAccept",
            "value": 803.474,
            "range": "± 4.347",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080035\nmean 804.018 ns"
          }
        ]
      },
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
          "id": "06c9dcd86539ac3365bc0bb6b3432db5ad769ce5",
          "message": "Merge pull request #7 from assimalign/fix/record-both-metrics-on-alert\n\nRecord allocations even when the time series alerts",
          "timestamp": "2026-10-19T20:26:47Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/06c9dcd86539ac3365bc0bb6b3432db5ad769ce5"
        },
        "date": 1792448700618,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "InMemoryConnectionBenchmarks.RoundTrip_SystemIOPipelines(PayloadSize=64)",
            "value": 261.243,
            "range": "± 1.135",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080329\nmean 261.405 ns"
          },
          {
            "name": "InMemoryConnectionBenchmarks.RoundTrip(PayloadSize=64)",
            "value": 263.107,
            "range": "± 6.068",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080329\nmean 262.964 ns"
          },
          {
            "name": "InMemoryConnectionBenchmarks.RoundTrip_SystemIOPipelines(PayloadSize=4096)",
            "value": 344.849,
            "range": "± 6.688",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080329\nmean 343.572 ns"
          },
          {
            "name": "InMemoryConnectionBenchmarks.RoundTrip(PayloadSize=4096)",
            "value": 342.699,
            "range": "± 3.409",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080329\nmean 342.167 ns"
          },
          {
            "name": "InMemoryConnectionListenerBenchmarks.ConnectAccept",
            "value": 557.057,
            "range": "± 10.922",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080329\nmean 554.282 ns"
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
      },
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
          "id": "00e8cb49c3ca66a73eac2bf997d449bd83a7801a",
          "message": "Merge pull request #5 from assimalign/fix/benchmarks-ci-first-run\n\nfix(benchmarks): first measuring run failures (history bootstrap, keep-alive close, Python stall)",
          "timestamp": "2026-10-19T10:03:39Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/00e8cb49c3ca66a73eac2bf997d449bd83a7801a"
        },
        "date": 1792416680867,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "InMemoryConnectionBenchmarks.RoundTrip_SystemIOPipelines(PayloadSize=64) - allocated",
            "value": 144,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080035"
          },
          {
            "name": "InMemoryConnectionBenchmarks.RoundTrip(PayloadSize=64) - allocated",
            "value": 144,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080035"
          },
          {
            "name": "InMemoryConnectionBenchmarks.RoundTrip_SystemIOPipelines(PayloadSize=4096) - allocated",
            "value": 144,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080035"
          },
          {
            "name": "InMemoryConnectionBenchmarks.RoundTrip(PayloadSize=4096) - allocated",
            "value": 144,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080035"
          },
          {
            "name": "InMemoryConnectionListenerBenchmarks.ConnectAccept - allocated",
            "value": 1848,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080035"
          }
        ]
      },
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
          "id": "06c9dcd86539ac3365bc0bb6b3432db5ad769ce5",
          "message": "Merge pull request #7 from assimalign/fix/record-both-metrics-on-alert\n\nRecord allocations even when the time series alerts",
          "timestamp": "2026-10-19T20:26:47Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/06c9dcd86539ac3365bc0bb6b3432db5ad769ce5"
        },
        "date": 1792448702941,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "InMemoryConnectionBenchmarks.RoundTrip_SystemIOPipelines(PayloadSize=64) - allocated",
            "value": 144,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080329"
          },
          {
            "name": "InMemoryConnectionBenchmarks.RoundTrip(PayloadSize=64) - allocated",
            "value": 144,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080329"
          },
          {
            "name": "InMemoryConnectionBenchmarks.RoundTrip_SystemIOPipelines(PayloadSize=4096) - allocated",
            "value": 144,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080329"
          },
          {
            "name": "InMemoryConnectionBenchmarks.RoundTrip(PayloadSize=4096) - allocated",
            "value": 144,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080329"
          },
          {
            "name": "InMemoryConnectionListenerBenchmarks.ConnectAccept - allocated",
            "value": 1848,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080329"
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
      },
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
          "id": "00e8cb49c3ca66a73eac2bf997d449bd83a7801a",
          "message": "Merge pull request #5 from assimalign/fix/benchmarks-ci-first-run\n\nfix(benchmarks): first measuring run failures (history bootstrap, keep-alive close, Python stall)",
          "timestamp": "2026-10-19T10:03:39Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/00e8cb49c3ca66a73eac2bf997d449bd83a7801a"
        },
        "date": 1792416694476,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "BmffDocumentBenchmarks.Load",
            "value": 15209.613,
            "range": "± 40.925",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080037\nmean 15217.504 ns"
          },
          {
            "name": "MarkdownBenchmarks.Parse_Markdig",
            "value": 19600.712,
            "range": "± 33.529",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080037\nmean 19598.97 ns"
          },
          {
            "name": "MarkdownBenchmarks.Parse",
            "value": 62732.152,
            "range": "± 279.874",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080037\nmean 62713.298 ns"
          },
          {
            "name": "MarkdownBenchmarks.ToHtml_Markdig",
            "value": 23193.283,
            "range": "± 21.597",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080037\nmean 23194.812 ns"
          },
          {
            "name": "MarkdownBenchmarks.ToHtml",
            "value": 72599.072,
            "range": "± 163.386",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080037\nmean 72583.395 ns"
          },
          {
            "name": "YamlBenchmarks.Parse_YamlDotNet",
            "value": 57382.154,
            "range": "± 79.942",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080037\nmean 57394.268 ns"
          },
          {
            "name": "YamlBenchmarks.Parse",
            "value": 22298.767,
            "range": "± 51.267",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080037\nmean 22294.857 ns"
          }
        ]
      },
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
          "id": "06c9dcd86539ac3365bc0bb6b3432db5ad769ce5",
          "message": "Merge pull request #7 from assimalign/fix/record-both-metrics-on-alert\n\nRecord allocations even when the time series alerts",
          "timestamp": "2026-10-19T20:26:47Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/06c9dcd86539ac3365bc0bb6b3432db5ad769ce5"
        },
        "date": 1792448715819,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "BmffDocumentBenchmarks.Load",
            "value": 7891.301,
            "range": "± 131.292",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080331\nmean 7926.493 ns"
          },
          {
            "name": "MarkdownBenchmarks.Parse_Markdig",
            "value": 10661.086,
            "range": "± 109.38",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080331\nmean 10625.547 ns"
          },
          {
            "name": "MarkdownBenchmarks.Parse",
            "value": 31835.512,
            "range": "± 616.944",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080331\nmean 32078.328 ns"
          },
          {
            "name": "MarkdownBenchmarks.ToHtml_Markdig",
            "value": 12501.78,
            "range": "± 140.669",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080331\nmean 12504.556 ns"
          },
          {
            "name": "MarkdownBenchmarks.ToHtml",
            "value": 35650.091,
            "range": "± 523.049",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080331\nmean 35703.417 ns"
          },
          {
            "name": "YamlBenchmarks.Parse_YamlDotNet",
            "value": 31404.626,
            "range": "± 462.437",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080331\nmean 31402.073 ns"
          },
          {
            "name": "YamlBenchmarks.Parse",
            "value": 10143.156,
            "range": "± 313.584",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080331\nmean 10189.413 ns"
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
      },
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
          "id": "00e8cb49c3ca66a73eac2bf997d449bd83a7801a",
          "message": "Merge pull request #5 from assimalign/fix/benchmarks-ci-first-run\n\nfix(benchmarks): first measuring run failures (history bootstrap, keep-alive close, Python stall)",
          "timestamp": "2026-10-19T10:03:39Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/00e8cb49c3ca66a73eac2bf997d449bd83a7801a"
        },
        "date": 1792416697080,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "BmffDocumentBenchmarks.Load - allocated",
            "value": 19168,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080037"
          },
          {
            "name": "MarkdownBenchmarks.Parse_Markdig - allocated",
            "value": 19984,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080037"
          },
          {
            "name": "MarkdownBenchmarks.Parse - allocated",
            "value": 127657,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080037"
          },
          {
            "name": "MarkdownBenchmarks.ToHtml_Markdig - allocated",
            "value": 24224,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080037"
          },
          {
            "name": "MarkdownBenchmarks.ToHtml - allocated",
            "value": 147665,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080037"
          },
          {
            "name": "YamlBenchmarks.Parse_YamlDotNet - allocated",
            "value": 55928,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080037"
          },
          {
            "name": "YamlBenchmarks.Parse - allocated",
            "value": 46072,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080037"
          }
        ]
      },
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
          "id": "06c9dcd86539ac3365bc0bb6b3432db5ad769ce5",
          "message": "Merge pull request #7 from assimalign/fix/record-both-metrics-on-alert\n\nRecord allocations even when the time series alerts",
          "timestamp": "2026-10-19T20:26:47Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/06c9dcd86539ac3365bc0bb6b3432db5ad769ce5"
        },
        "date": 1792448718410,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "BmffDocumentBenchmarks.Load - allocated",
            "value": 19168,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080331"
          },
          {
            "name": "MarkdownBenchmarks.Parse_Markdig - allocated",
            "value": 19984,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080331"
          },
          {
            "name": "MarkdownBenchmarks.Parse - allocated",
            "value": 127656,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080331"
          },
          {
            "name": "MarkdownBenchmarks.ToHtml_Markdig - allocated",
            "value": 24224,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080331"
          },
          {
            "name": "MarkdownBenchmarks.ToHtml - allocated",
            "value": 147664,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080331"
          },
          {
            "name": "YamlBenchmarks.Parse_YamlDotNet - allocated",
            "value": 55928,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080331"
          },
          {
            "name": "YamlBenchmarks.Parse - allocated",
            "value": 46072,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080331"
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
      },
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
          "id": "00e8cb49c3ca66a73eac2bf997d449bd83a7801a",
          "message": "Merge pull request #5 from assimalign/fix/benchmarks-ci-first-run\n\nfix(benchmarks): first measuring run failures (history bootstrap, keep-alive close, Python stall)",
          "timestamp": "2026-10-19T10:03:39Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/00e8cb49c3ca66a73eac2bf997d449bd83a7801a"
        },
        "date": 1792416714206,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "FileSystemPathBenchmarks.GetSegments",
            "value": 178.129,
            "range": "± 2.005",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080039\nmean 178.526 ns"
          },
          {
            "name": "FileSystemPathBenchmarks.Parse(value=//tes(...)e.txt [33])",
            "value": 2563.986,
            "range": "± 47.233",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080039\nmean 2591.018 ns"
          },
          {
            "name": "FileSystemPathBenchmarks.Parse(value=C:/te(...)e.txt [34])",
            "value": 2919.027,
            "range": "± 52.09",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080039\nmean 2917.587 ns"
          },
          {
            "name": "GlobBenchmarks.IsMatch_Regex(Pattern=p?th/(...)].txt [21])",
            "value": 1887.341,
            "range": "± 11.935",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080039\nmean 1884.025 ns"
          },
          {
            "name": "GlobBenchmarks.IsMatch(Pattern=p?th/(...)].txt [21])",
            "value": 5906.789,
            "range": "± 14.814",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080039\nmean 5908.018 ns"
          },
          {
            "name": "GlobBenchmarks.IsMatch_Regex(Pattern=src/**/*.{cs,json})",
            "value": 3256.143,
            "range": "± 6.521",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080039\nmean 3257.613 ns"
          },
          {
            "name": "GlobBenchmarks.IsMatch(Pattern=src/**/*.{cs,json})",
            "value": 11466.509,
            "range": "± 38.001",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080039\nmean 11465.63 ns"
          }
        ]
      },
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
          "id": "06c9dcd86539ac3365bc0bb6b3432db5ad769ce5",
          "message": "Merge pull request #7 from assimalign/fix/record-both-metrics-on-alert\n\nRecord allocations even when the time series alerts",
          "timestamp": "2026-10-19T20:26:47Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/06c9dcd86539ac3365bc0bb6b3432db5ad769ce5"
        },
        "date": 1792448729470,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "FileSystemPathBenchmarks.GetSegments",
            "value": 153.309,
            "range": "± 1.968",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080335\nmean 154.007 ns"
          },
          {
            "name": "FileSystemPathBenchmarks.Parse(value=//tes(...)e.txt [33])",
            "value": 1788.435,
            "range": "± 41.931",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080335\nmean 1777.054 ns"
          },
          {
            "name": "FileSystemPathBenchmarks.Parse(value=C:/te(...)e.txt [34])",
            "value": 1974.534,
            "range": "± 236.07",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080335\nmean 1910.219 ns"
          },
          {
            "name": "GlobBenchmarks.IsMatch_Regex(Pattern=p?th/(...)].txt [21])",
            "value": 1184.239,
            "range": "± 17.339",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080335\nmean 1191.112 ns"
          },
          {
            "name": "GlobBenchmarks.IsMatch(Pattern=p?th/(...)].txt [21])",
            "value": 2913.011,
            "range": "± 198.952",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080335\nmean 3035.786 ns"
          },
          {
            "name": "GlobBenchmarks.IsMatch_Regex(Pattern=src/**/*.{cs,json})",
            "value": 2052.801,
            "range": "± 104.952",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080335\nmean 2116.24 ns"
          },
          {
            "name": "GlobBenchmarks.IsMatch(Pattern=src/**/*.{cs,json})",
            "value": 5546.051,
            "range": "± 176.863",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080335\nmean 5638.95 ns"
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
      },
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
          "id": "00e8cb49c3ca66a73eac2bf997d449bd83a7801a",
          "message": "Merge pull request #5 from assimalign/fix/benchmarks-ci-first-run\n\nfix(benchmarks): first measuring run failures (history bootstrap, keep-alive close, Python stall)",
          "timestamp": "2026-10-19T10:03:39Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/00e8cb49c3ca66a73eac2bf997d449bd83a7801a"
        },
        "date": 1792416718090,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "FileSystemPathBenchmarks.GetSegments - allocated",
            "value": 520,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080039"
          },
          {
            "name": "FileSystemPathBenchmarks.Parse(value=//tes(...)e.txt [33]) - allocated",
            "value": 3904,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080039"
          },
          {
            "name": "FileSystemPathBenchmarks.Parse(value=C:/te(...)e.txt [34]) - allocated",
            "value": 4272,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080039"
          },
          {
            "name": "GlobBenchmarks.IsMatch_Regex(Pattern=p?th/(...)].txt [21]) - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080039"
          },
          {
            "name": "GlobBenchmarks.IsMatch(Pattern=p?th/(...)].txt [21]) - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080039"
          },
          {
            "name": "GlobBenchmarks.IsMatch_Regex(Pattern=src/**/*.{cs,json}) - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080039"
          },
          {
            "name": "GlobBenchmarks.IsMatch(Pattern=src/**/*.{cs,json}) - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080039"
          }
        ]
      },
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
          "id": "06c9dcd86539ac3365bc0bb6b3432db5ad769ce5",
          "message": "Merge pull request #7 from assimalign/fix/record-both-metrics-on-alert\n\nRecord allocations even when the time series alerts",
          "timestamp": "2026-10-19T20:26:47Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/06c9dcd86539ac3365bc0bb6b3432db5ad769ce5"
        },
        "date": 1792448731172,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "FileSystemPathBenchmarks.GetSegments - allocated",
            "value": 520,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080335"
          },
          {
            "name": "FileSystemPathBenchmarks.Parse(value=//tes(...)e.txt [33]) - allocated",
            "value": 3904,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080335"
          },
          {
            "name": "FileSystemPathBenchmarks.Parse(value=C:/te(...)e.txt [34]) - allocated",
            "value": 4272,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080335"
          },
          {
            "name": "GlobBenchmarks.IsMatch_Regex(Pattern=p?th/(...)].txt [21]) - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080335"
          },
          {
            "name": "GlobBenchmarks.IsMatch(Pattern=p?th/(...)].txt [21]) - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080335"
          },
          {
            "name": "GlobBenchmarks.IsMatch_Regex(Pattern=src/**/*.{cs,json}) - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080335"
          },
          {
            "name": "GlobBenchmarks.IsMatch(Pattern=src/**/*.{cs,json}) - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080335"
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
      },
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
          "id": "00e8cb49c3ca66a73eac2bf997d449bd83a7801a",
          "message": "Merge pull request #5 from assimalign/fix/benchmarks-ci-first-run\n\nfix(benchmarks): first measuring run failures (history bootstrap, keep-alive close, Python stall)",
          "timestamp": "2026-10-19T10:03:39Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/00e8cb49c3ca66a73eac2bf997d449bd83a7801a"
        },
        "date": 1792416731868,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "ServiceProviderBuildBenchmarks.BuildAndResolve_MicrosoftExtensions",
            "value": 3156.351,
            "range": "± 58.185",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080041\nmean 3170.681 ns"
          },
          {
            "name": "ServiceProviderBuildBenchmarks.BuildAndResolve",
            "value": 2400.432,
            "range": "± 25.772",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080041\nmean 2389.994 ns"
          },
          {
            "name": "ServiceResolutionBenchmarks.CreateScopeAndResolve_MicrosoftExtensions",
            "value": 134.064,
            "range": "± 1.509",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080041\nmean 134.273 ns"
          },
          {
            "name": "ServiceResolutionBenchmarks.CreateScopeAndResolve",
            "value": 143.631,
            "range": "± 1.039",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080041\nmean 143.83 ns"
          },
          {
            "name": "ServiceResolutionBenchmarks.ResolveSingleton_MicrosoftExtensions",
            "value": 13.802,
            "range": "± 0.011",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080041\nmean 13.804 ns"
          },
          {
            "name": "ServiceResolutionBenchmarks.ResolveSingleton",
            "value": 15.431,
            "range": "± 0.016",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080041\nmean 15.427 ns"
          },
          {
            "name": "ServiceResolutionBenchmarks.ResolveTransient_MicrosoftExtensions",
            "value": 72.758,
            "range": "± 0.201",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080041\nmean 72.739 ns"
          },
          {
            "name": "ServiceResolutionBenchmarks.ResolveTransient",
            "value": 62.692,
            "range": "± 0.127",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080041\nmean 62.685 ns"
          }
        ]
      },
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
          "id": "06c9dcd86539ac3365bc0bb6b3432db5ad769ce5",
          "message": "Merge pull request #7 from assimalign/fix/record-both-metrics-on-alert\n\nRecord allocations even when the time series alerts",
          "timestamp": "2026-10-19T20:26:47Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/06c9dcd86539ac3365bc0bb6b3432db5ad769ce5"
        },
        "date": 1792448749544,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "ServiceProviderBuildBenchmarks.BuildAndResolve_MicrosoftExtensions",
            "value": 3190.808,
            "range": "± 21.433",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080339\nmean 3194.547 ns"
          },
          {
            "name": "ServiceProviderBuildBenchmarks.BuildAndResolve",
            "value": 2448.664,
            "range": "± 27.542",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080339\nmean 2450.802 ns"
          },
          {
            "name": "ServiceResolutionBenchmarks.CreateScopeAndResolve_MicrosoftExtensions",
            "value": 137.579,
            "range": "± 1.529",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080339\nmean 137.693 ns"
          },
          {
            "name": "ServiceResolutionBenchmarks.CreateScopeAndResolve",
            "value": 139.169,
            "range": "± 1.121",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080339\nmean 138.961 ns"
          },
          {
            "name": "ServiceResolutionBenchmarks.ResolveSingleton_MicrosoftExtensions",
            "value": 14.706,
            "range": "± 0.025",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080339\nmean 14.713 ns"
          },
          {
            "name": "ServiceResolutionBenchmarks.ResolveSingleton",
            "value": 16.299,
            "range": "± 0.131",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080339\nmean 16.36 ns"
          },
          {
            "name": "ServiceResolutionBenchmarks.ResolveTransient_MicrosoftExtensions",
            "value": 73.063,
            "range": "± 0.07",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080339\nmean 73.062 ns"
          },
          {
            "name": "ServiceResolutionBenchmarks.ResolveTransient",
            "value": 60.781,
            "range": "± 0.211",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080339\nmean 60.847 ns"
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
      },
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
          "id": "00e8cb49c3ca66a73eac2bf997d449bd83a7801a",
          "message": "Merge pull request #5 from assimalign/fix/benchmarks-ci-first-run\n\nfix(benchmarks): first measuring run failures (history bootstrap, keep-alive close, Python stall)",
          "timestamp": "2026-10-19T10:03:39Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/00e8cb49c3ca66a73eac2bf997d449bd83a7801a"
        },
        "date": 1792416734660,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "ServiceProviderBuildBenchmarks.BuildAndResolve_MicrosoftExtensions - allocated",
            "value": 6296,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080041"
          },
          {
            "name": "ServiceProviderBuildBenchmarks.BuildAndResolve - allocated",
            "value": 5960,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080041"
          },
          {
            "name": "ServiceResolutionBenchmarks.CreateScopeAndResolve_MicrosoftExtensions - allocated",
            "value": 368,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080041"
          },
          {
            "name": "ServiceResolutionBenchmarks.CreateScopeAndResolve - allocated",
            "value": 344,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080041"
          },
          {
            "name": "ServiceResolutionBenchmarks.ResolveSingleton_MicrosoftExtensions - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080041"
          },
          {
            "name": "ServiceResolutionBenchmarks.ResolveSingleton - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080041"
          },
          {
            "name": "ServiceResolutionBenchmarks.ResolveTransient_MicrosoftExtensions - allocated",
            "value": 32,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080041"
          },
          {
            "name": "ServiceResolutionBenchmarks.ResolveTransient - allocated",
            "value": 32,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080041"
          }
        ]
      },
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
          "id": "06c9dcd86539ac3365bc0bb6b3432db5ad769ce5",
          "message": "Merge pull request #7 from assimalign/fix/record-both-metrics-on-alert\n\nRecord allocations even when the time series alerts",
          "timestamp": "2026-10-19T20:26:47Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/06c9dcd86539ac3365bc0bb6b3432db5ad769ce5"
        },
        "date": 1792448752316,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "ServiceProviderBuildBenchmarks.BuildAndResolve_MicrosoftExtensions - allocated",
            "value": 6296,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080339"
          },
          {
            "name": "ServiceProviderBuildBenchmarks.BuildAndResolve - allocated",
            "value": 5960,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080339"
          },
          {
            "name": "ServiceResolutionBenchmarks.CreateScopeAndResolve_MicrosoftExtensions - allocated",
            "value": 368,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080339"
          },
          {
            "name": "ServiceResolutionBenchmarks.CreateScopeAndResolve - allocated",
            "value": 344,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080339"
          },
          {
            "name": "ServiceResolutionBenchmarks.ResolveSingleton_MicrosoftExtensions - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080339"
          },
          {
            "name": "ServiceResolutionBenchmarks.ResolveSingleton - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080339"
          },
          {
            "name": "ServiceResolutionBenchmarks.ResolveTransient_MicrosoftExtensions - allocated",
            "value": 32,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080339"
          },
          {
            "name": "ServiceResolutionBenchmarks.ResolveTransient - allocated",
            "value": 32,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080339"
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
      },
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
          "id": "00e8cb49c3ca66a73eac2bf997d449bd83a7801a",
          "message": "Merge pull request #5 from assimalign/fix/benchmarks-ci-first-run\n\nfix(benchmarks): first measuring run failures (history bootstrap, keep-alive close, Python stall)",
          "timestamp": "2026-10-19T10:03:39Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/00e8cb49c3ca66a73eac2bf997d449bd83a7801a"
        },
        "date": 1792416752818,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "DnsMessageBenchmarks.WriteQuery",
            "value": 440.779,
            "range": "± 3.021",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080042\nmean 440.224 ns"
          },
          {
            "name": "DnsMessageBenchmarks.ParseResponse",
            "value": 1152.8,
            "range": "± 7.856",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080042\nmean 1152.429 ns"
          },
          {
            "name": "DnsMessageBenchmarks.WriteResponse",
            "value": 2680.121,
            "range": "± 33.083",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080042\nmean 2681.683 ns"
          }
        ]
      },
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
          "id": "06c9dcd86539ac3365bc0bb6b3432db5ad769ce5",
          "message": "Merge pull request #7 from assimalign/fix/record-both-metrics-on-alert\n\nRecord allocations even when the time series alerts",
          "timestamp": "2026-10-19T20:26:47Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/06c9dcd86539ac3365bc0bb6b3432db5ad769ce5"
        },
        "date": 1792448766924,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "DnsMessageBenchmarks.WriteQuery",
            "value": 225.585,
            "range": "± 7.593",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080343\nmean 226.785 ns"
          },
          {
            "name": "DnsMessageBenchmarks.ParseResponse",
            "value": 583.757,
            "range": "± 8.231",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080343\nmean 586.214 ns"
          },
          {
            "name": "DnsMessageBenchmarks.WriteResponse",
            "value": 1412.416,
            "range": "± 21.719",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080343\nmean 1403.993 ns"
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
      },
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
          "id": "00e8cb49c3ca66a73eac2bf997d449bd83a7801a",
          "message": "Merge pull request #5 from assimalign/fix/benchmarks-ci-first-run\n\nfix(benchmarks): first measuring run failures (history bootstrap, keep-alive close, Python stall)",
          "timestamp": "2026-10-19T10:03:39Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/00e8cb49c3ca66a73eac2bf997d449bd83a7801a"
        },
        "date": 1792416756032,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "DnsMessageBenchmarks.WriteQuery - allocated",
            "value": 848,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080042"
          },
          {
            "name": "DnsMessageBenchmarks.ParseResponse - allocated",
            "value": 2632,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080042"
          },
          {
            "name": "DnsMessageBenchmarks.WriteResponse - allocated",
            "value": 5520,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080042"
          }
        ]
      },
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
          "id": "06c9dcd86539ac3365bc0bb6b3432db5ad769ce5",
          "message": "Merge pull request #7 from assimalign/fix/record-both-metrics-on-alert\n\nRecord allocations even when the time series alerts",
          "timestamp": "2026-10-19T20:26:47Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/06c9dcd86539ac3365bc0bb6b3432db5ad769ce5"
        },
        "date": 1792448769631,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "DnsMessageBenchmarks.WriteQuery - allocated",
            "value": 848,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080343"
          },
          {
            "name": "DnsMessageBenchmarks.ParseResponse - allocated",
            "value": 2632,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080343"
          },
          {
            "name": "DnsMessageBenchmarks.WriteResponse - allocated",
            "value": 5520,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080343"
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
      },
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
          "id": "00e8cb49c3ca66a73eac2bf997d449bd83a7801a",
          "message": "Merge pull request #5 from assimalign/fix/benchmarks-ci-first-run\n\nfix(benchmarks): first measuring run failures (history bootstrap, keep-alive close, Python stall)",
          "timestamp": "2026-10-19T10:03:39Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/00e8cb49c3ca66a73eac2bf997d449bd83a7801a"
        },
        "date": 1792416773287,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "GlobMatcherBenchmarks.Filter_MicrosoftExtensions",
            "value": 66851.211,
            "range": "± 1338.031",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080045\nmean 67064.512 ns"
          },
          {
            "name": "GlobMatcherBenchmarks.Filter",
            "value": 26293.843,
            "range": "± 350.356",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080045\nmean 26211.663 ns"
          },
          {
            "name": "GlobMatcherBenchmarks.Filter_FromStrings",
            "value": 242097.865,
            "range": "± 2504.948",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080045\nmean 242259.351 ns"
          },
          {
            "name": "InMemoryFileSystemBenchmarks.ReadFile",
            "value": 6966.647,
            "range": "± 139.162",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080045\nmean 6994.291 ns"
          },
          {
            "name": "PhysicalFileSystemBenchmarks.ReadFile_SystemIO",
            "value": 5485.358,
            "range": "± 24.516",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080045\nmean 5480.209 ns"
          },
          {
            "name": "PhysicalFileSystemBenchmarks.ReadFile",
            "value": 10794.961,
            "range": "± 77.729",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080045\nmean 10800.032 ns"
          }
        ]
      },
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
          "id": "06c9dcd86539ac3365bc0bb6b3432db5ad769ce5",
          "message": "Merge pull request #7 from assimalign/fix/record-both-metrics-on-alert\n\nRecord allocations even when the time series alerts",
          "timestamp": "2026-10-19T20:26:47Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/06c9dcd86539ac3365bc0bb6b3432db5ad769ce5"
        },
        "date": 1792448784397,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "GlobMatcherBenchmarks.Filter_MicrosoftExtensions",
            "value": 79740.295,
            "range": "± 519.583",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080347\nmean 79777.021 ns"
          },
          {
            "name": "GlobMatcherBenchmarks.Filter",
            "value": 29267.807,
            "range": "± 132.598",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080347\nmean 29260.224 ns"
          },
          {
            "name": "GlobMatcherBenchmarks.Filter_FromStrings",
            "value": 347075.088,
            "range": "± 3959.319",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080347\nmean 347965.421 ns"
          },
          {
            "name": "InMemoryFileSystemBenchmarks.ReadFile",
            "value": 9595.184,
            "range": "± 96.02",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080347\nmean 9573.802 ns"
          },
          {
            "name": "PhysicalFileSystemBenchmarks.ReadFile_SystemIO",
            "value": 2232.253,
            "range": "± 26.331",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080347\nmean 2236.553 ns"
          },
          {
            "name": "PhysicalFileSystemBenchmarks.ReadFile",
            "value": 7064.441,
            "range": "± 137.471",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080347\nmean 7094.121 ns"
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
      },
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
          "id": "00e8cb49c3ca66a73eac2bf997d449bd83a7801a",
          "message": "Merge pull request #5 from assimalign/fix/benchmarks-ci-first-run\n\nfix(benchmarks): first measuring run failures (history bootstrap, keep-alive close, Python stall)",
          "timestamp": "2026-10-19T10:03:39Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/00e8cb49c3ca66a73eac2bf997d449bd83a7801a"
        },
        "date": 1792416775653,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "GlobMatcherBenchmarks.Filter_MicrosoftExtensions - allocated",
            "value": 186416,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080045"
          },
          {
            "name": "GlobMatcherBenchmarks.Filter - allocated",
            "value": 13760,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080045"
          },
          {
            "name": "GlobMatcherBenchmarks.Filter_FromStrings - allocated",
            "value": 883136,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080045"
          },
          {
            "name": "InMemoryFileSystemBenchmarks.ReadFile - allocated",
            "value": 23872,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080045"
          },
          {
            "name": "PhysicalFileSystemBenchmarks.ReadFile_SystemIO - allocated",
            "value": 232,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080045"
          },
          {
            "name": "PhysicalFileSystemBenchmarks.ReadFile - allocated",
            "value": 8936,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080045"
          }
        ]
      },
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
          "id": "06c9dcd86539ac3365bc0bb6b3432db5ad769ce5",
          "message": "Merge pull request #7 from assimalign/fix/record-both-metrics-on-alert\n\nRecord allocations even when the time series alerts",
          "timestamp": "2026-10-19T20:26:47Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/06c9dcd86539ac3365bc0bb6b3432db5ad769ce5"
        },
        "date": 1792448787065,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "GlobMatcherBenchmarks.Filter_MicrosoftExtensions - allocated",
            "value": 186416,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080347"
          },
          {
            "name": "GlobMatcherBenchmarks.Filter - allocated",
            "value": 13760,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080347"
          },
          {
            "name": "GlobMatcherBenchmarks.Filter_FromStrings - allocated",
            "value": 883138,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080347"
          },
          {
            "name": "InMemoryFileSystemBenchmarks.ReadFile - allocated",
            "value": 23872,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080347"
          },
          {
            "name": "PhysicalFileSystemBenchmarks.ReadFile_SystemIO - allocated",
            "value": 232,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080347"
          },
          {
            "name": "PhysicalFileSystemBenchmarks.ReadFile - allocated",
            "value": 8936,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080347"
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
      },
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
          "id": "00e8cb49c3ca66a73eac2bf997d449bd83a7801a",
          "message": "Merge pull request #5 from assimalign/fix/benchmarks-ci-first-run\n\nfix(benchmarks): first measuring run failures (history bootstrap, keep-alive close, Python stall)",
          "timestamp": "2026-10-19T10:03:39Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/00e8cb49c3ca66a73eac2bf997d449bd83a7801a"
        },
        "date": 1792416792395,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "HostLifecycleBenchmarks.BuildStartStop_MicrosoftExtensions(ServiceCount=1)",
            "value": 218899.909,
            "range": "± 14588.08",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080047\nmean 218788.375 ns"
          },
          {
            "name": "HostLifecycleBenchmarks.BuildStartStop(ServiceCount=1)",
            "value": 2163.485,
            "range": "± 19.815",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080047\nmean 2172.124 ns"
          },
          {
            "name": "HostLifecycleBenchmarks.BuildStartStop_MicrosoftExtensions(ServiceCount=16)",
            "value": 243801.07,
            "range": "± 5966.473",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080047\nmean 243925.529 ns"
          },
          {
            "name": "HostLifecycleBenchmarks.BuildStartStop(ServiceCount=16)",
            "value": 6240.466,
            "range": "± 24.246",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080047\nmean 6241.051 ns"
          }
        ]
      },
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
          "id": "06c9dcd86539ac3365bc0bb6b3432db5ad769ce5",
          "message": "Merge pull request #7 from assimalign/fix/record-both-metrics-on-alert\n\nRecord allocations even when the time series alerts",
          "timestamp": "2026-10-19T20:26:47Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/06c9dcd86539ac3365bc0bb6b3432db5ad769ce5"
        },
        "date": 1792448805136,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "HostLifecycleBenchmarks.BuildStartStop_MicrosoftExtensions(ServiceCount=1)",
            "value": 96725.891,
            "range": "± 18998.447",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080348\nmean 104028.946 ns"
          },
          {
            "name": "HostLifecycleBenchmarks.BuildStartStop(ServiceCount=1)",
            "value": 1630.638,
            "range": "± 16.093",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080348\nmean 1635.322 ns"
          },
          {
            "name": "HostLifecycleBenchmarks.BuildStartStop_MicrosoftExtensions(ServiceCount=16)",
            "value": 151852.209,
            "range": "± 2711.766",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080348\nmean 151562.032 ns"
          },
          {
            "name": "HostLifecycleBenchmarks.BuildStartStop(ServiceCount=16)",
            "value": 4667.035,
            "range": "± 73.208",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080348\nmean 4652.855 ns"
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
      },
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
          "id": "00e8cb49c3ca66a73eac2bf997d449bd83a7801a",
          "message": "Merge pull request #5 from assimalign/fix/benchmarks-ci-first-run\n\nfix(benchmarks): first measuring run failures (history bootstrap, keep-alive close, Python stall)",
          "timestamp": "2026-10-19T10:03:39Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/00e8cb49c3ca66a73eac2bf997d449bd83a7801a"
        },
        "date": 1792416795091,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "HostLifecycleBenchmarks.BuildStartStop_MicrosoftExtensions(ServiceCount=1) - allocated",
            "value": 69431,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080047"
          },
          {
            "name": "HostLifecycleBenchmarks.BuildStartStop(ServiceCount=1) - allocated",
            "value": 2336,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080047"
          },
          {
            "name": "HostLifecycleBenchmarks.BuildStartStop_MicrosoftExtensions(ServiceCount=16) - allocated",
            "value": 91873,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080047"
          },
          {
            "name": "HostLifecycleBenchmarks.BuildStartStop(ServiceCount=16) - allocated",
            "value": 9056,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080047"
          }
        ]
      },
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
          "id": "06c9dcd86539ac3365bc0bb6b3432db5ad769ce5",
          "message": "Merge pull request #7 from assimalign/fix/record-both-metrics-on-alert\n\nRecord allocations even when the time series alerts",
          "timestamp": "2026-10-19T20:26:47Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/06c9dcd86539ac3365bc0bb6b3432db5ad769ce5"
        },
        "date": 1792448807895,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "HostLifecycleBenchmarks.BuildStartStop_MicrosoftExtensions(ServiceCount=1) - allocated",
            "value": 74265,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080348"
          },
          {
            "name": "HostLifecycleBenchmarks.BuildStartStop(ServiceCount=1) - allocated",
            "value": 2344,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080348"
          },
          {
            "name": "HostLifecycleBenchmarks.BuildStartStop_MicrosoftExtensions(ServiceCount=16) - allocated",
            "value": 96462,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080348"
          },
          {
            "name": "HostLifecycleBenchmarks.BuildStartStop(ServiceCount=16) - allocated",
            "value": 9064,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080348"
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
      },
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
          "id": "00e8cb49c3ca66a73eac2bf997d449bd83a7801a",
          "message": "Merge pull request #5 from assimalign/fix/benchmarks-ci-first-run\n\nfix(benchmarks): first measuring run failures (history bootstrap, keep-alive close, Python stall)",
          "timestamp": "2026-10-19T10:03:39Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/00e8cb49c3ca66a73eac2bf997d449bd83a7801a"
        },
        "date": 1792416812790,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "Http1RequestBenchmarks.ServeRequest(Header=bearer)",
            "value": 1829.179,
            "range": "± 8.518",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080048\nmean 1828.863 ns"
          },
          {
            "name": "Http1RequestBenchmarks.ServeRequest(Header=cookie)",
            "value": 1819.565,
            "range": "± 11.012",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080048\nmean 1821.273 ns"
          },
          {
            "name": "Http1RequestBenchmarks.ServeRequest(Header=none)",
            "value": 1543.935,
            "range": "± 11.266",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080048\nmean 1545.069 ns"
          },
          {
            "name": "HttpCookieBenchmarks.FormatSetCookie_AspNetCore",
            "value": 64.63,
            "range": "± 0.358",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080048\nmean 64.667 ns"
          },
          {
            "name": "HttpCookieBenchmarks.FormatSetCookie",
            "value": 115.723,
            "range": "± 9.491",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080048\nmean 117.192 ns"
          },
          {
            "name": "HttpCookieBenchmarks.ParseCookieHeader_AspNetCore",
            "value": 402.874,
            "range": "± 26.054",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080048\nmean 414.512 ns"
          },
          {
            "name": "HttpCookieBenchmarks.ParseCookieHeader",
            "value": 354.913,
            "range": "± 5.252",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080048\nmean 354.404 ns"
          },
          {
            "name": "HttpAcceptParserBenchmarks.ParseAccept_AspNetCore",
            "value": 857.477,
            "range": "± 15.221",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080048\nmean 858.158 ns"
          },
          {
            "name": "HttpAcceptParserBenchmarks.ParseAccept",
            "value": 801.851,
            "range": "± 19.51",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080048\nmean 803.354 ns"
          },
          {
            "name": "HttpForwardedBenchmarks.ParseForwarded",
            "value": 694.047,
            "range": "± 5.696",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080048\nmean 694.512 ns"
          },
          {
            "name": "HttpForwardedBenchmarks.ParseXForwardedFor",
            "value": 218.917,
            "range": "± 1.736",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080048\nmean 219.511 ns"
          }
        ]
      },
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
          "id": "06c9dcd86539ac3365bc0bb6b3432db5ad769ce5",
          "message": "Merge pull request #7 from assimalign/fix/record-both-metrics-on-alert\n\nRecord allocations even when the time series alerts",
          "timestamp": "2026-10-19T20:26:47Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/06c9dcd86539ac3365bc0bb6b3432db5ad769ce5"
        },
        "date": 1792448825225,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "Http1RequestBenchmarks.ServeRequest(Header=bearer)",
            "value": 3476.915,
            "range": "± 15.63",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080352\nmean 3483.266 ns"
          },
          {
            "name": "Http1RequestBenchmarks.ServeRequest(Header=cookie)",
            "value": 3533.603,
            "range": "± 16.528",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080352\nmean 3533.951 ns"
          },
          {
            "name": "Http1RequestBenchmarks.ServeRequest(Header=none)",
            "value": 2921.454,
            "range": "± 9.303",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080352\nmean 2919.963 ns"
          },
          {
            "name": "HttpCookieBenchmarks.FormatSetCookie_AspNetCore",
            "value": 148.93,
            "range": "± 1.056",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080352\nmean 148.681 ns"
          },
          {
            "name": "HttpCookieBenchmarks.FormatSetCookie",
            "value": 240.01,
            "range": "± 3.906",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080352\nmean 239.382 ns"
          },
          {
            "name": "HttpCookieBenchmarks.ParseCookieHeader_AspNetCore",
            "value": 744.414,
            "range": "± 2.236",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080352\nmean 743.946 ns"
          },
          {
            "name": "HttpCookieBenchmarks.ParseCookieHeader",
            "value": 742.949,
            "range": "± 6.708",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080352\nmean 741.782 ns"
          },
          {
            "name": "HttpAcceptParserBenchmarks.ParseAccept_AspNetCore",
            "value": 1837.522,
            "range": "± 11.853",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080352\nmean 1839.809 ns"
          },
          {
            "name": "HttpAcceptParserBenchmarks.ParseAccept",
            "value": 1604.632,
            "range": "± 11.396",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080352\nmean 1610.069 ns"
          },
          {
            "name": "HttpForwardedBenchmarks.ParseForwarded",
            "value": 1438.844,
            "range": "± 6.237",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080352\nmean 1439.658 ns"
          },
          {
            "name": "HttpForwardedBenchmarks.ParseXForwardedFor",
            "value": 441.404,
            "range": "± 3.168",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080352\nmean 442.545 ns"
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
      },
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
          "id": "00e8cb49c3ca66a73eac2bf997d449bd83a7801a",
          "message": "Merge pull request #5 from assimalign/fix/benchmarks-ci-first-run\n\nfix(benchmarks): first measuring run failures (history bootstrap, keep-alive close, Python stall)",
          "timestamp": "2026-10-19T10:03:39Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/00e8cb49c3ca66a73eac2bf997d449bd83a7801a"
        },
        "date": 1792416816212,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "Http1RequestBenchmarks.ServeRequest(Header=bearer) - allocated",
            "value": 4776,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080048"
          },
          {
            "name": "Http1RequestBenchmarks.ServeRequest(Header=cookie) - allocated",
            "value": 5496,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080048"
          },
          {
            "name": "Http1RequestBenchmarks.ServeRequest(Header=none) - allocated",
            "value": 3272,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080048"
          },
          {
            "name": "HttpCookieBenchmarks.FormatSetCookie_AspNetCore - allocated",
            "value": 360,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080048"
          },
          {
            "name": "HttpCookieBenchmarks.FormatSetCookie - allocated",
            "value": 1040,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080048"
          },
          {
            "name": "HttpCookieBenchmarks.ParseCookieHeader_AspNetCore - allocated",
            "value": 496,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080048"
          },
          {
            "name": "HttpCookieBenchmarks.ParseCookieHeader - allocated",
            "value": 2176,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080048"
          },
          {
            "name": "HttpAcceptParserBenchmarks.ParseAccept_AspNetCore - allocated",
            "value": 1672,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080048"
          },
          {
            "name": "HttpAcceptParserBenchmarks.ParseAccept - allocated",
            "value": 2384,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080048"
          },
          {
            "name": "HttpForwardedBenchmarks.ParseForwarded - allocated",
            "value": 2144,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080048"
          },
          {
            "name": "HttpForwardedBenchmarks.ParseXForwardedFor - allocated",
            "value": 696,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080048"
          }
        ]
      },
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
          "id": "06c9dcd86539ac3365bc0bb6b3432db5ad769ce5",
          "message": "Merge pull request #7 from assimalign/fix/record-both-metrics-on-alert\n\nRecord allocations even when the time series alerts",
          "timestamp": "2026-10-19T20:26:47Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/06c9dcd86539ac3365bc0bb6b3432db5ad769ce5"
        },
        "date": 1792448828594,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "Http1RequestBenchmarks.ServeRequest(Header=bearer) - allocated",
            "value": 4776,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080352"
          },
          {
            "name": "Http1RequestBenchmarks.ServeRequest(Header=cookie) - allocated",
            "value": 5496,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080352"
          },
          {
            "name": "Http1RequestBenchmarks.ServeRequest(Header=none) - allocated",
            "value": 3272,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080352"
          },
          {
            "name": "HttpCookieBenchmarks.FormatSetCookie_AspNetCore - allocated",
            "value": 360,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080352"
          },
          {
            "name": "HttpCookieBenchmarks.FormatSetCookie - allocated",
            "value": 1040,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080352"
          },
          {
            "name": "HttpCookieBenchmarks.ParseCookieHeader_AspNetCore - allocated",
            "value": 496,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080352"
          },
          {
            "name": "HttpCookieBenchmarks.ParseCookieHeader - allocated",
            "value": 2176,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080352"
          },
          {
            "name": "HttpAcceptParserBenchmarks.ParseAccept_AspNetCore - allocated",
            "value": 1672,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080352"
          },
          {
            "name": "HttpAcceptParserBenchmarks.ParseAccept - allocated",
            "value": 2384,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080352"
          },
          {
            "name": "HttpForwardedBenchmarks.ParseForwarded - allocated",
            "value": 2144,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080352"
          },
          {
            "name": "HttpForwardedBenchmarks.ParseXForwardedFor - allocated",
            "value": 696,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080352"
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
      },
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
          "id": "00e8cb49c3ca66a73eac2bf997d449bd83a7801a",
          "message": "Merge pull request #5 from assimalign/fix/benchmarks-ci-first-run\n\nfix(benchmarks): first measuring run failures (history bootstrap, keep-alive close, Python stall)",
          "timestamp": "2026-10-19T10:03:39Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/00e8cb49c3ca66a73eac2bf997d449bd83a7801a"
        },
        "date": 1792416834262,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "JsonWebTokenBenchmarks.Parse_MicrosoftIdentityModel",
            "value": 1784.088,
            "range": "± 6.186",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080050\nmean 1786.326 ns"
          },
          {
            "name": "JsonWebTokenBenchmarks.Parse",
            "value": 3158.009,
            "range": "± 16.534",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080050\nmean 3157.997 ns"
          },
          {
            "name": "JsonWebTokenBenchmarks.Validate_MicrosoftIdentityModel",
            "value": 77875.991,
            "range": "± 123.105",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080050\nmean 77927.563 ns"
          },
          {
            "name": "JsonWebTokenBenchmarks.Validate",
            "value": 241055.438,
            "range": "± 343.511",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080050\nmean 241152.979 ns"
          },
          {
            "name": "JsonWebTokenBenchmarks.Write_MicrosoftIdentityModel",
            "value": 29736.57,
            "range": "± 21.025",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080050\nmean 29735.996 ns"
          },
          {
            "name": "JsonWebTokenBenchmarks.Write",
            "value": 30201.037,
            "range": "± 31.125",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080050\nmean 30210.422 ns"
          }
        ]
      },
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
          "id": "06c9dcd86539ac3365bc0bb6b3432db5ad769ce5",
          "message": "Merge pull request #7 from assimalign/fix/record-both-metrics-on-alert\n\nRecord allocations even when the time series alerts",
          "timestamp": "2026-10-19T20:26:47Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/06c9dcd86539ac3365bc0bb6b3432db5ad769ce5"
        },
        "date": 1792448845214,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "JsonWebTokenBenchmarks.Parse_MicrosoftIdentityModel",
            "value": 1533.212,
            "range": "± 11.431",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080353\nmean 1535.133 ns"
          },
          {
            "name": "JsonWebTokenBenchmarks.Parse",
            "value": 2987.129,
            "range": "± 12.158",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080353\nmean 2986.367 ns"
          },
          {
            "name": "JsonWebTokenBenchmarks.Validate_MicrosoftIdentityModel",
            "value": 82737.867,
            "range": "± 228.061",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080353\nmean 82706.39 ns"
          },
          {
            "name": "JsonWebTokenBenchmarks.Validate",
            "value": 250821.975,
            "range": "± 1052.873",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080353\nmean 250723.092 ns"
          },
          {
            "name": "JsonWebTokenBenchmarks.Write_MicrosoftIdentityModel",
            "value": 28972.849,
            "range": "± 63.797",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080353\nmean 28983.477 ns"
          },
          {
            "name": "JsonWebTokenBenchmarks.Write",
            "value": 29422.303,
            "range": "± 101.5",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080353\nmean 29466.268 ns"
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
      },
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
          "id": "00e8cb49c3ca66a73eac2bf997d449bd83a7801a",
          "message": "Merge pull request #5 from assimalign/fix/benchmarks-ci-first-run\n\nfix(benchmarks): first measuring run failures (history bootstrap, keep-alive close, Python stall)",
          "timestamp": "2026-10-19T10:03:39Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/00e8cb49c3ca66a73eac2bf997d449bd83a7801a"
        },
        "date": 1792416837132,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "JsonWebTokenBenchmarks.Parse_MicrosoftIdentityModel - allocated",
            "value": 2520,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080050"
          },
          {
            "name": "JsonWebTokenBenchmarks.Parse - allocated",
            "value": 5784,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080050"
          },
          {
            "name": "JsonWebTokenBenchmarks.Validate_MicrosoftIdentityModel - allocated",
            "value": 5073,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080050"
          },
          {
            "name": "JsonWebTokenBenchmarks.Validate - allocated",
            "value": 10554,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080050"
          },
          {
            "name": "JsonWebTokenBenchmarks.Write_MicrosoftIdentityModel - allocated",
            "value": 2056,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080050"
          },
          {
            "name": "JsonWebTokenBenchmarks.Write - allocated",
            "value": 5032,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080050"
          }
        ]
      },
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
          "id": "06c9dcd86539ac3365bc0bb6b3432db5ad769ce5",
          "message": "Merge pull request #7 from assimalign/fix/record-both-metrics-on-alert\n\nRecord allocations even when the time series alerts",
          "timestamp": "2026-10-19T20:26:47Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/06c9dcd86539ac3365bc0bb6b3432db5ad769ce5"
        },
        "date": 1792448848122,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "JsonWebTokenBenchmarks.Parse_MicrosoftIdentityModel - allocated",
            "value": 2520,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080353"
          },
          {
            "name": "JsonWebTokenBenchmarks.Parse - allocated",
            "value": 5784,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080353"
          },
          {
            "name": "JsonWebTokenBenchmarks.Validate_MicrosoftIdentityModel - allocated",
            "value": 5072,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080353"
          },
          {
            "name": "JsonWebTokenBenchmarks.Validate - allocated",
            "value": 10554,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080353"
          },
          {
            "name": "JsonWebTokenBenchmarks.Write_MicrosoftIdentityModel - allocated",
            "value": 2056,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080353"
          },
          {
            "name": "JsonWebTokenBenchmarks.Write - allocated",
            "value": 5032,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080353"
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
      },
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
          "id": "00e8cb49c3ca66a73eac2bf997d449bd83a7801a",
          "message": "Merge pull request #5 from assimalign/fix/benchmarks-ci-first-run\n\nfix(benchmarks): first measuring run failures (history bootstrap, keep-alive close, Python stall)",
          "timestamp": "2026-10-19T10:03:39Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/00e8cb49c3ca66a73eac2bf997d449bd83a7801a"
        },
        "date": 1792416852647,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "LoggerBenchmarks.LogDisabled_MicrosoftExtensions",
            "value": 26.02,
            "range": "± 0.097",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080052\nmean 26.019 ns"
          },
          {
            "name": "LoggerBenchmarks.LogDisabled",
            "value": 35.134,
            "range": "± 0.731",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080052\nmean 34.876 ns"
          },
          {
            "name": "LoggerBenchmarks.LogEnabled_MicrosoftExtensions",
            "value": 51.188,
            "range": "± 0.546",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080052\nmean 51.057 ns"
          },
          {
            "name": "LoggerBenchmarks.LogEnabled",
            "value": 172.497,
            "range": "± 0.401",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080052\nmean 172.428 ns"
          },
          {
            "name": "LoggerBenchmarks.Scope_MicrosoftExtensions",
            "value": 37.142,
            "range": "± 0.111",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080052\nmean 37.158 ns"
          },
          {
            "name": "LoggerBenchmarks.Scope",
            "value": 291.013,
            "range": "± 1.168",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080052\nmean 291.064 ns"
          }
        ]
      },
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
          "id": "06c9dcd86539ac3365bc0bb6b3432db5ad769ce5",
          "message": "Merge pull request #7 from assimalign/fix/record-both-metrics-on-alert\n\nRecord allocations even when the time series alerts",
          "timestamp": "2026-10-19T20:26:47Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/06c9dcd86539ac3365bc0bb6b3432db5ad769ce5"
        },
        "date": 1792448864043,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "LoggerBenchmarks.LogDisabled_MicrosoftExtensions",
            "value": 23.639,
            "range": "± 0.127",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080354\nmean 23.634 ns"
          },
          {
            "name": "LoggerBenchmarks.LogDisabled",
            "value": 23.566,
            "range": "± 0.255",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080354\nmean 23.605 ns"
          },
          {
            "name": "LoggerBenchmarks.LogEnabled_MicrosoftExtensions",
            "value": 40.976,
            "range": "± 0.366",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080354\nmean 41.098 ns"
          },
          {
            "name": "LoggerBenchmarks.LogEnabled",
            "value": 126.485,
            "range": "± 0.617",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080354\nmean 126.459 ns"
          },
          {
            "name": "LoggerBenchmarks.Scope_MicrosoftExtensions",
            "value": 35.985,
            "range": "± 0.08",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080354\nmean 35.977 ns"
          },
          {
            "name": "LoggerBenchmarks.Scope",
            "value": 223.623,
            "range": "± 1.096",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080354\nmean 223.864 ns"
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
      },
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
          "id": "00e8cb49c3ca66a73eac2bf997d449bd83a7801a",
          "message": "Merge pull request #5 from assimalign/fix/benchmarks-ci-first-run\n\nfix(benchmarks): first measuring run failures (history bootstrap, keep-alive close, Python stall)",
          "timestamp": "2026-10-19T10:03:39Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/00e8cb49c3ca66a73eac2bf997d449bd83a7801a"
        },
        "date": 1792416855140,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "LoggerBenchmarks.LogDisabled_MicrosoftExtensions - allocated",
            "value": 32,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080052"
          },
          {
            "name": "LoggerBenchmarks.LogDisabled - allocated",
            "value": 216,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080052"
          },
          {
            "name": "LoggerBenchmarks.LogEnabled_MicrosoftExtensions - allocated",
            "value": 88,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080052"
          },
          {
            "name": "LoggerBenchmarks.LogEnabled - allocated",
            "value": 368,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080052"
          },
          {
            "name": "LoggerBenchmarks.Scope_MicrosoftExtensions - allocated",
            "value": 24,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080052"
          },
          {
            "name": "LoggerBenchmarks.Scope - allocated",
            "value": 456,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080052"
          }
        ]
      },
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
          "id": "06c9dcd86539ac3365bc0bb6b3432db5ad769ce5",
          "message": "Merge pull request #7 from assimalign/fix/record-both-metrics-on-alert\n\nRecord allocations even when the time series alerts",
          "timestamp": "2026-10-19T20:26:47Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/06c9dcd86539ac3365bc0bb6b3432db5ad769ce5"
        },
        "date": 1792448866620,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "LoggerBenchmarks.LogDisabled_MicrosoftExtensions - allocated",
            "value": 32,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080354"
          },
          {
            "name": "LoggerBenchmarks.LogDisabled - allocated",
            "value": 216,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080354"
          },
          {
            "name": "LoggerBenchmarks.LogEnabled_MicrosoftExtensions - allocated",
            "value": 88,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080354"
          },
          {
            "name": "LoggerBenchmarks.LogEnabled - allocated",
            "value": 368,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080354"
          },
          {
            "name": "LoggerBenchmarks.Scope_MicrosoftExtensions - allocated",
            "value": 24,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080354"
          },
          {
            "name": "LoggerBenchmarks.Scope - allocated",
            "value": 456,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080354"
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
      },
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
          "id": "00e8cb49c3ca66a73eac2bf997d449bd83a7801a",
          "message": "Merge pull request #5 from assimalign/fix/benchmarks-ci-first-run\n\nfix(benchmarks): first measuring run failures (history bootstrap, keep-alive close, Python stall)",
          "timestamp": "2026-10-19T10:03:39Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/00e8cb49c3ca66a73eac2bf997d449bd83a7801a"
        },
        "date": 1792416869781,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "MapperBenchmarks.MapFlat_AutoMapper",
            "value": 74.359,
            "range": "± 0.385",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080053\nmean 74.331 ns"
          },
          {
            "name": "MapperBenchmarks.MapFlat",
            "value": 165.817,
            "range": "± 0.857",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080053\nmean 166.039 ns"
          },
          {
            "name": "MapperBenchmarks.MapNested_AutoMapper",
            "value": 319.124,
            "range": "± 2.666",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080053\nmean 319.002 ns"
          },
          {
            "name": "MapperBenchmarks.MapNested",
            "value": 1813.254,
            "range": "± 6.216",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080053\nmean 1815.138 ns"
          }
        ]
      },
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
          "id": "06c9dcd86539ac3365bc0bb6b3432db5ad769ce5",
          "message": "Merge pull request #7 from assimalign/fix/record-both-metrics-on-alert\n\nRecord allocations even when the time series alerts",
          "timestamp": "2026-10-19T20:26:47Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/06c9dcd86539ac3365bc0bb6b3432db5ad769ce5"
        },
        "date": 1792448879262,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "MapperBenchmarks.MapFlat_AutoMapper",
            "value": 60.52,
            "range": "± 0.282",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080364\nmean 60.461 ns"
          },
          {
            "name": "MapperBenchmarks.MapFlat",
            "value": 106.145,
            "range": "± 0.505",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080364\nmean 106.081 ns"
          },
          {
            "name": "MapperBenchmarks.MapNested_AutoMapper",
            "value": 240.835,
            "range": "± 4.419",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080364\nmean 239.305 ns"
          },
          {
            "name": "MapperBenchmarks.MapNested",
            "value": 1273.259,
            "range": "± 4.96",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080364\nmean 1271.944 ns"
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
      },
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
          "id": "06c9dcd86539ac3365bc0bb6b3432db5ad769ce5",
          "message": "Merge pull request #7 from assimalign/fix/record-both-metrics-on-alert\n\nRecord allocations even when the time series alerts",
          "timestamp": "2026-10-19T20:26:47Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/06c9dcd86539ac3365bc0bb6b3432db5ad769ce5"
        },
        "date": 1792448881143,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "MapperBenchmarks.MapFlat_AutoMapper - allocated",
            "value": 56,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080364"
          },
          {
            "name": "MapperBenchmarks.MapFlat - allocated",
            "value": 144,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080364"
          },
          {
            "name": "MapperBenchmarks.MapNested_AutoMapper - allocated",
            "value": 952,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080364"
          },
          {
            "name": "MapperBenchmarks.MapNested - allocated",
            "value": 2288,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080364"
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
      },
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
          "id": "00e8cb49c3ca66a73eac2bf997d449bd83a7801a",
          "message": "Merge pull request #5 from assimalign/fix/benchmarks-ci-first-run\n\nfix(benchmarks): first measuring run failures (history bootstrap, keep-alive close, Python stall)",
          "timestamp": "2026-10-19T10:03:39Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/00e8cb49c3ca66a73eac2bf997d449bd83a7801a"
        },
        "date": 1792416884799,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "DefaultObjectPoolBenchmarks.RentReturn_MicrosoftExtensions",
            "value": 11.554,
            "range": "± 0.047",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080056\nmean 11.564 ns"
          },
          {
            "name": "DefaultObjectPoolBenchmarks.RentReturn",
            "value": 10.956,
            "range": "± 0.02",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080056\nmean 10.965 ns"
          },
          {
            "name": "DefaultObjectPoolBenchmarks.RentTwoReturnTwo",
            "value": 27.511,
            "range": "± 0.089",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080056\nmean 27.49 ns"
          }
        ]
      },
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
          "id": "06c9dcd86539ac3365bc0bb6b3432db5ad769ce5",
          "message": "Merge pull request #7 from assimalign/fix/record-both-metrics-on-alert\n\nRecord allocations even when the time series alerts",
          "timestamp": "2026-10-19T20:26:47Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/06c9dcd86539ac3365bc0bb6b3432db5ad769ce5"
        },
        "date": 1792448894021,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "DefaultObjectPoolBenchmarks.RentReturn_MicrosoftExtensions",
            "value": 8.921,
            "range": "± 0.019",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080366\nmean 8.928 ns"
          },
          {
            "name": "DefaultObjectPoolBenchmarks.RentReturn",
            "value": 8.872,
            "range": "± 0.145",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080366\nmean 8.877 ns"
          },
          {
            "name": "DefaultObjectPoolBenchmarks.RentTwoReturnTwo",
            "value": 23.795,
            "range": "± 0.017",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080366\nmean 23.796 ns"
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
      },
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
          "id": "00e8cb49c3ca66a73eac2bf997d449bd83a7801a",
          "message": "Merge pull request #5 from assimalign/fix/benchmarks-ci-first-run\n\nfix(benchmarks): first measuring run failures (history bootstrap, keep-alive close, Python stall)",
          "timestamp": "2026-10-19T10:03:39Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/00e8cb49c3ca66a73eac2bf997d449bd83a7801a"
        },
        "date": 1792416886801,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "DefaultObjectPoolBenchmarks.RentReturn_MicrosoftExtensions - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080056"
          },
          {
            "name": "DefaultObjectPoolBenchmarks.RentReturn - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080056"
          },
          {
            "name": "DefaultObjectPoolBenchmarks.RentTwoReturnTwo - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080056"
          }
        ]
      },
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
          "id": "06c9dcd86539ac3365bc0bb6b3432db5ad769ce5",
          "message": "Merge pull request #7 from assimalign/fix/record-both-metrics-on-alert\n\nRecord allocations even when the time series alerts",
          "timestamp": "2026-10-19T20:26:47Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/06c9dcd86539ac3365bc0bb6b3432db5ad769ce5"
        },
        "date": 1792448896650,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "DefaultObjectPoolBenchmarks.RentReturn_MicrosoftExtensions - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080366"
          },
          {
            "name": "DefaultObjectPoolBenchmarks.RentReturn - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080366"
          },
          {
            "name": "DefaultObjectPoolBenchmarks.RentTwoReturnTwo - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080366"
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
      },
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
          "id": "00e8cb49c3ca66a73eac2bf997d449bd83a7801a",
          "message": "Merge pull request #5 from assimalign/fix/benchmarks-ci-first-run\n\nfix(benchmarks): first measuring run failures (history bootstrap, keep-alive close, Python stall)",
          "timestamp": "2026-10-19T10:03:39Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/00e8cb49c3ca66a73eac2bf997d449bd83a7801a"
        },
        "date": 1792416900750,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "ValidatorBenchmarks.ValidateComparisons_FluentValidation",
            "value": 4219.304,
            "range": "± 47.902",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080058\nmean 4214.074 ns"
          },
          {
            "name": "ValidatorBenchmarks.ValidateComparisons",
            "value": 2942.127,
            "range": "± 52.979",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080058\nmean 2939.024 ns"
          },
          {
            "name": "ValidatorBenchmarks.ValidateInvalid_FluentValidation",
            "value": 3999.041,
            "range": "± 60.432",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080058\nmean 4000.566 ns"
          },
          {
            "name": "ValidatorBenchmarks.ValidateInvalid",
            "value": 3196.63,
            "range": "± 71.861",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080058\nmean 3203.044 ns"
          },
          {
            "name": "ValidatorBenchmarks.Validate_FluentValidation",
            "value": 971.343,
            "range": "± 6.374",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080058\nmean 972.964 ns"
          },
          {
            "name": "ValidatorBenchmarks.Validate",
            "value": 3694.436,
            "range": "± 90.685",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080058\nmean 3688.21 ns"
          }
        ]
      },
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
          "id": "06c9dcd86539ac3365bc0bb6b3432db5ad769ce5",
          "message": "Merge pull request #7 from assimalign/fix/record-both-metrics-on-alert\n\nRecord allocations even when the time series alerts",
          "timestamp": "2026-10-19T20:26:47Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/06c9dcd86539ac3365bc0bb6b3432db5ad769ce5"
        },
        "date": 1792448911314,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "ValidatorBenchmarks.ValidateComparisons_FluentValidation",
            "value": 4426.549,
            "range": "± 51.084",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080371\nmean 4417.519 ns"
          },
          {
            "name": "ValidatorBenchmarks.ValidateComparisons",
            "value": 3383.749,
            "range": "± 53.719",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080371\nmean 3380.024 ns"
          },
          {
            "name": "ValidatorBenchmarks.ValidateInvalid_FluentValidation",
            "value": 4097.836,
            "range": "± 100.645",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080371\nmean 4088.285 ns"
          },
          {
            "name": "ValidatorBenchmarks.ValidateInvalid",
            "value": 3277.966,
            "range": "± 63.189",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080371\nmean 3285.719 ns"
          },
          {
            "name": "ValidatorBenchmarks.Validate_FluentValidation",
            "value": 1003.397,
            "range": "± 8.909",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080371\nmean 999.834 ns"
          },
          {
            "name": "ValidatorBenchmarks.Validate",
            "value": 3703.863,
            "range": "± 90.826",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080371\nmean 3690.773 ns"
          }
        ]
      }
    ],
    "libraries/ObjectValidation - allocations": [
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
        "date": 1792403228960,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "ValidatorBenchmarks.ValidateComparisons_FluentValidation - allocated",
            "value": 8584,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079886"
          },
          {
            "name": "ValidatorBenchmarks.ValidateComparisons - allocated",
            "value": 17672,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079886"
          },
          {
            "name": "ValidatorBenchmarks.ValidateInvalid_FluentValidation - allocated",
            "value": 8456,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079886"
          },
          {
            "name": "ValidatorBenchmarks.ValidateInvalid - allocated",
            "value": 18216,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079886"
          },
          {
            "name": "ValidatorBenchmarks.Validate_FluentValidation - allocated",
            "value": 1624,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079886"
          },
          {
            "name": "ValidatorBenchmarks.Validate - allocated",
            "value": 20152,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079886"
          }
        ]
      },
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
          "id": "00e8cb49c3ca66a73eac2bf997d449bd83a7801a",
          "message": "Merge pull request #5 from assimalign/fix/benchmarks-ci-first-run\n\nfix(benchmarks): first measuring run failures (history bootstrap, keep-alive close, Python stall)",
          "timestamp": "2026-10-19T10:03:39Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/00e8cb49c3ca66a73eac2bf997d449bd83a7801a"
        },
        "date": 1792416903072,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "ValidatorBenchmarks.ValidateComparisons_FluentValidation - allocated",
            "value": 8584,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080058"
          },
          {
            "name": "ValidatorBenchmarks.ValidateComparisons - allocated",
            "value": 17672,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080058"
          },
          {
            "name": "ValidatorBenchmarks.ValidateInvalid_FluentValidation - allocated",
            "value": 8456,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080058"
          },
          {
            "name": "ValidatorBenchmarks.ValidateInvalid - allocated",
            "value": 18216,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080058"
          },
          {
            "name": "ValidatorBenchmarks.Validate_FluentValidation - allocated",
            "value": 1624,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080058"
          },
          {
            "name": "ValidatorBenchmarks.Validate - allocated",
            "value": 20152,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080058"
          }
        ]
      },
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
          "id": "06c9dcd86539ac3365bc0bb6b3432db5ad769ce5",
          "message": "Merge pull request #7 from assimalign/fix/record-both-metrics-on-alert\n\nRecord allocations even when the time series alerts",
          "timestamp": "2026-10-19T20:26:47Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/06c9dcd86539ac3365bc0bb6b3432db5ad769ce5"
        },
        "date": 1792448913817,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "ValidatorBenchmarks.ValidateComparisons_FluentValidation - allocated",
            "value": 8584,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080371"
          },
          {
            "name": "ValidatorBenchmarks.ValidateComparisons - allocated",
            "value": 17672,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080371"
          },
          {
            "name": "ValidatorBenchmarks.ValidateInvalid_FluentValidation - allocated",
            "value": 8456,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080371"
          },
          {
            "name": "ValidatorBenchmarks.ValidateInvalid - allocated",
            "value": 18216,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080371"
          },
          {
            "name": "ValidatorBenchmarks.Validate_FluentValidation - allocated",
            "value": 1624,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080371"
          },
          {
            "name": "ValidatorBenchmarks.Validate - allocated",
            "value": 20152,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080371"
          }
        ]
      }
    ],
    "libraries/OpenApi - time": [
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
        "date": 1792403241880,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "OpenApiJsonBenchmarks.Parse_MicrosoftOpenApi",
            "value": 215102.628,
            "range": "± 4498.698",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079889\nmean 217130.827 ns"
          },
          {
            "name": "OpenApiJsonBenchmarks.Parse",
            "value": 75430.152,
            "range": "± 505.391",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079889\nmean 75354.398 ns"
          },
          {
            "name": "OpenApiJsonBenchmarks.Serialize_MicrosoftOpenApi",
            "value": 49280.845,
            "range": "± 193.268",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079889\nmean 49310.128 ns"
          },
          {
            "name": "OpenApiJsonBenchmarks.Serialize",
            "value": 38335.067,
            "range": "± 326.613",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079889\nmean 38425.773 ns"
          }
        ]
      },
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
          "id": "00e8cb49c3ca66a73eac2bf997d449bd83a7801a",
          "message": "Merge pull request #5 from assimalign/fix/benchmarks-ci-first-run\n\nfix(benchmarks): first measuring run failures (history bootstrap, keep-alive close, Python stall)",
          "timestamp": "2026-10-19T10:03:39Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/00e8cb49c3ca66a73eac2bf997d449bd83a7801a"
        },
        "date": 1792416918709,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "OpenApiJsonBenchmarks.Parse_MicrosoftOpenApi",
            "value": 88659.882,
            "range": "± 6553.805",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080059\nmean 91488.532 ns"
          },
          {
            "name": "OpenApiJsonBenchmarks.Parse",
            "value": 37555.888,
            "range": "± 642.52",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080059\nmean 37506.118 ns"
          },
          {
            "name": "OpenApiJsonBenchmarks.Serialize_MicrosoftOpenApi",
            "value": 25679.416,
            "range": "± 465.65",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080059\nmean 25796.205 ns"
          },
          {
            "name": "OpenApiJsonBenchmarks.Serialize",
            "value": 19499.873,
            "range": "± 209.428",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080059\nmean 19490.163 ns"
          }
        ]
      },
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
          "id": "06c9dcd86539ac3365bc0bb6b3432db5ad769ce5",
          "message": "Merge pull request #7 from assimalign/fix/record-both-metrics-on-alert\n\nRecord allocations even when the time series alerts",
          "timestamp": "2026-10-19T20:26:47Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/06c9dcd86539ac3365bc0bb6b3432db5ad769ce5"
        },
        "date": 1792448928649,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "OpenApiJsonBenchmarks.Parse_MicrosoftOpenApi",
            "value": 122934.923,
            "range": "± 3069.609",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080373\nmean 122323.77 ns"
          },
          {
            "name": "OpenApiJsonBenchmarks.Parse",
            "value": 51752.81,
            "range": "± 1127.623",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080373\nmean 52164.922 ns"
          },
          {
            "name": "OpenApiJsonBenchmarks.Serialize_MicrosoftOpenApi",
            "value": 33398.261,
            "range": "± 581.162",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080373\nmean 33589.155 ns"
          },
          {
            "name": "OpenApiJsonBenchmarks.Serialize",
            "value": 27442.188,
            "range": "± 704.122",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080373\nmean 27544.66 ns"
          }
        ]
      }
    ],
    "libraries/OpenApi - allocations": [
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
        "date": 1792403243681,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "OpenApiJsonBenchmarks.Parse_MicrosoftOpenApi - allocated",
            "value": 192440,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079889"
          },
          {
            "name": "OpenApiJsonBenchmarks.Parse - allocated",
            "value": 102144,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079889"
          },
          {
            "name": "OpenApiJsonBenchmarks.Serialize_MicrosoftOpenApi - allocated",
            "value": 63736,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079889"
          },
          {
            "name": "OpenApiJsonBenchmarks.Serialize - allocated",
            "value": 75080,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079889"
          }
        ]
      },
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
          "id": "00e8cb49c3ca66a73eac2bf997d449bd83a7801a",
          "message": "Merge pull request #5 from assimalign/fix/benchmarks-ci-first-run\n\nfix(benchmarks): first measuring run failures (history bootstrap, keep-alive close, Python stall)",
          "timestamp": "2026-10-19T10:03:39Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/00e8cb49c3ca66a73eac2bf997d449bd83a7801a"
        },
        "date": 1792416921434,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "OpenApiJsonBenchmarks.Parse_MicrosoftOpenApi - allocated",
            "value": 192056,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080059"
          },
          {
            "name": "OpenApiJsonBenchmarks.Parse - allocated",
            "value": 102144,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080059"
          },
          {
            "name": "OpenApiJsonBenchmarks.Serialize_MicrosoftOpenApi - allocated",
            "value": 63736,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080059"
          },
          {
            "name": "OpenApiJsonBenchmarks.Serialize - allocated",
            "value": 75080,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080059"
          }
        ]
      },
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
          "id": "06c9dcd86539ac3365bc0bb6b3432db5ad769ce5",
          "message": "Merge pull request #7 from assimalign/fix/record-both-metrics-on-alert\n\nRecord allocations even when the time series alerts",
          "timestamp": "2026-10-19T20:26:47Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/06c9dcd86539ac3365bc0bb6b3432db5ad769ce5"
        },
        "date": 1792448930920,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "OpenApiJsonBenchmarks.Parse_MicrosoftOpenApi - allocated",
            "value": 192154,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080373"
          },
          {
            "name": "OpenApiJsonBenchmarks.Parse - allocated",
            "value": 102144,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080373"
          },
          {
            "name": "OpenApiJsonBenchmarks.Serialize_MicrosoftOpenApi - allocated",
            "value": 64056,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080373"
          },
          {
            "name": "OpenApiJsonBenchmarks.Serialize - allocated",
            "value": 75080,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080373"
          }
        ]
      }
    ],
    "libraries/OpenTelemetry - time": [
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
        "date": 1792403255426,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "OtlpLogExporterBenchmarks.ExportAsync(BatchSize=1)",
            "value": 2987.29,
            "range": "± 17.055",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079890\nmean 2988.857 ns"
          },
          {
            "name": "OtlpLogExporterBenchmarks.EnqueueThenFlush(BatchSize=1)",
            "value": 3763.276,
            "range": "± 28.344",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079890\nmean 3754.443 ns"
          },
          {
            "name": "OtlpLogExporterBenchmarks.ExportAsync(BatchSize=64)",
            "value": 67253.464,
            "range": "± 309.676",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079890\nmean 67248.804 ns"
          },
          {
            "name": "OtlpLogExporterBenchmarks.EnqueueThenFlush(BatchSize=64)",
            "value": 71192.608,
            "range": "± 279.255",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079890\nmean 71216.907 ns"
          }
        ]
      },
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
          "id": "00e8cb49c3ca66a73eac2bf997d449bd83a7801a",
          "message": "Merge pull request #5 from assimalign/fix/benchmarks-ci-first-run\n\nfix(benchmarks): first measuring run failures (history bootstrap, keep-alive close, Python stall)",
          "timestamp": "2026-10-19T10:03:39Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/00e8cb49c3ca66a73eac2bf997d449bd83a7801a"
        },
        "date": 1792416942287,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "OtlpLogExporterBenchmarks.ExportAsync(BatchSize=1)",
            "value": 2007.43,
            "range": "± 42.157",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080061\nmean 2013.327 ns"
          },
          {
            "name": "OtlpLogExporterBenchmarks.EnqueueThenFlush(BatchSize=1)",
            "value": 2386.502,
            "range": "± 108.094",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080061\nmean 2394.791 ns"
          },
          {
            "name": "OtlpLogExporterBenchmarks.ExportAsync(BatchSize=64)",
            "value": 41612.757,
            "range": "± 339.648",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080061\nmean 41658.079 ns"
          },
          {
            "name": "OtlpLogExporterBenchmarks.EnqueueThenFlush(BatchSize=64)",
            "value": 44650.955,
            "range": "± 871.314",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080061\nmean 44754.72 ns"
          }
        ]
      }
    ],
    "libraries/OpenTelemetry - allocations": [
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
        "date": 1792403258066,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "OtlpLogExporterBenchmarks.ExportAsync(BatchSize=1) - allocated",
            "value": 4616,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079890"
          },
          {
            "name": "OtlpLogExporterBenchmarks.EnqueueThenFlush(BatchSize=1) - allocated",
            "value": 8736,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079890"
          },
          {
            "name": "OtlpLogExporterBenchmarks.ExportAsync(BatchSize=64) - allocated",
            "value": 95296,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079890"
          },
          {
            "name": "OtlpLogExporterBenchmarks.EnqueueThenFlush(BatchSize=64) - allocated",
            "value": 99416,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079890"
          }
        ]
      },
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
          "id": "00e8cb49c3ca66a73eac2bf997d449bd83a7801a",
          "message": "Merge pull request #5 from assimalign/fix/benchmarks-ci-first-run\n\nfix(benchmarks): first measuring run failures (history bootstrap, keep-alive close, Python stall)",
          "timestamp": "2026-10-19T10:03:39Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/00e8cb49c3ca66a73eac2bf997d449bd83a7801a"
        },
        "date": 1792416944232,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "OtlpLogExporterBenchmarks.ExportAsync(BatchSize=1) - allocated",
            "value": 4616,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080061"
          },
          {
            "name": "OtlpLogExporterBenchmarks.EnqueueThenFlush(BatchSize=1) - allocated",
            "value": 8736,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080061"
          },
          {
            "name": "OtlpLogExporterBenchmarks.ExportAsync(BatchSize=64) - allocated",
            "value": 95296,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080061"
          },
          {
            "name": "OtlpLogExporterBenchmarks.EnqueueThenFlush(BatchSize=64) - allocated",
            "value": 99416,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080061"
          }
        ]
      }
    ],
    "libraries/Resilience - time": [
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
        "date": 1792403271656,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "CircuitBreakerPipelineBenchmarks.Execute_Polly",
            "value": 177.206,
            "range": "± 0.378",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079891\nmean 177.316 ns"
          },
          {
            "name": "CircuitBreakerPipelineBenchmarks.Execute",
            "value": 224.045,
            "range": "± 1.007",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079891\nmean 224.359 ns"
          },
          {
            "name": "RetryPipelineBenchmarks.Execute_Polly",
            "value": 156.546,
            "range": "± 0.166",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079891\nmean 156.551 ns"
          },
          {
            "name": "RetryPipelineBenchmarks.Execute",
            "value": 180.903,
            "range": "± 1.067",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079891\nmean 180.455 ns"
          },
          {
            "name": "TimeoutPipelineBenchmarks.Execute_Polly",
            "value": 161.011,
            "range": "± 0.344",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079891\nmean 161.068 ns"
          },
          {
            "name": "TimeoutPipelineBenchmarks.Execute",
            "value": 953.797,
            "range": "± 112.955",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079891\nmean 975.564 ns"
          }
        ]
      },
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
          "id": "00e8cb49c3ca66a73eac2bf997d449bd83a7801a",
          "message": "Merge pull request #5 from assimalign/fix/benchmarks-ci-first-run\n\nfix(benchmarks): first measuring run failures (history bootstrap, keep-alive close, Python stall)",
          "timestamp": "2026-10-19T10:03:39Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/00e8cb49c3ca66a73eac2bf997d449bd83a7801a"
        },
        "date": 1792416954852,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "CircuitBreakerPipelineBenchmarks.Execute_Polly",
            "value": 206.22,
            "range": "± 0.517",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080063\nmean 206.27 ns"
          },
          {
            "name": "CircuitBreakerPipelineBenchmarks.Execute",
            "value": 241.257,
            "range": "± 0.671",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080063\nmean 241.397 ns"
          },
          {
            "name": "RetryPipelineBenchmarks.Execute_Polly",
            "value": 175.68,
            "range": "± 0.666",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080063\nmean 175.876 ns"
          },
          {
            "name": "RetryPipelineBenchmarks.Execute",
            "value": 197.291,
            "range": "± 0.538",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080063\nmean 197.316 ns"
          },
          {
            "name": "TimeoutPipelineBenchmarks.Execute_Polly",
            "value": 193.504,
            "range": "± 0.458",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080063\nmean 193.629 ns"
          },
          {
            "name": "TimeoutPipelineBenchmarks.Execute",
            "value": 1156.894,
            "range": "± 108.691",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080063\nmean 1146.8 ns"
          }
        ]
      }
    ],
    "libraries/Resilience - allocations": [
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
        "date": 1792403274302,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "CircuitBreakerPipelineBenchmarks.Execute_Polly - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079891"
          },
          {
            "name": "CircuitBreakerPipelineBenchmarks.Execute - allocated",
            "value": 200,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079891"
          },
          {
            "name": "RetryPipelineBenchmarks.Execute_Polly - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079891"
          },
          {
            "name": "RetryPipelineBenchmarks.Execute - allocated",
            "value": 112,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079891"
          },
          {
            "name": "TimeoutPipelineBenchmarks.Execute_Polly - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079891"
          },
          {
            "name": "TimeoutPipelineBenchmarks.Execute - allocated",
            "value": 560,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079891"
          }
        ]
      },
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
          "id": "00e8cb49c3ca66a73eac2bf997d449bd83a7801a",
          "message": "Merge pull request #5 from assimalign/fix/benchmarks-ci-first-run\n\nfix(benchmarks): first measuring run failures (history bootstrap, keep-alive close, Python stall)",
          "timestamp": "2026-10-19T10:03:39Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/00e8cb49c3ca66a73eac2bf997d449bd83a7801a"
        },
        "date": 1792416956936,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "CircuitBreakerPipelineBenchmarks.Execute_Polly - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080063"
          },
          {
            "name": "CircuitBreakerPipelineBenchmarks.Execute - allocated",
            "value": 200,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080063"
          },
          {
            "name": "RetryPipelineBenchmarks.Execute_Polly - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080063"
          },
          {
            "name": "RetryPipelineBenchmarks.Execute - allocated",
            "value": 112,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080063"
          },
          {
            "name": "TimeoutPipelineBenchmarks.Execute_Polly - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080063"
          },
          {
            "name": "TimeoutPipelineBenchmarks.Execute - allocated",
            "value": 560,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080063"
          }
        ]
      }
    ],
    "libraries/Security - time": [
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
        "date": 1792403291664,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "DataProtectorBenchmarks.Protect_AspNetCore(PayloadSize=64)",
            "value": 7605.648,
            "range": "± 154.904",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079892\nmean 7644.279 ns"
          },
          {
            "name": "DataProtectorBenchmarks.Protect(PayloadSize=64)",
            "value": 4608.667,
            "range": "± 59.712",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079892\nmean 4603.362 ns"
          },
          {
            "name": "DataProtectorBenchmarks.Protect_AspNetCore(PayloadSize=1024)",
            "value": 8897.264,
            "range": "± 55.885",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079892\nmean 8897.131 ns"
          },
          {
            "name": "DataProtectorBenchmarks.Protect(PayloadSize=1024)",
            "value": 4632.579,
            "range": "± 41.8",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079892\nmean 4644.975 ns"
          },
          {
            "name": "DataProtectorBenchmarks.Unprotect_AspNetCore(PayloadSize=64)",
            "value": 5700.369,
            "range": "± 16.497",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079892\nmean 5706.167 ns"
          },
          {
            "name": "DataProtectorBenchmarks.Unprotect(PayloadSize=64)",
            "value": 3643.651,
            "range": "± 41.672",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079892\nmean 3636.831 ns"
          },
          {
            "name": "DataProtectorBenchmarks.Unprotect_AspNetCore(PayloadSize=1024)",
            "value": 6467.465,
            "range": "± 24.299",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079892\nmean 6470.438 ns"
          },
          {
            "name": "DataProtectorBenchmarks.Unprotect(PayloadSize=1024)",
            "value": 3775.842,
            "range": "± 30.328",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079892\nmean 3774.083 ns"
          }
        ]
      },
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
          "id": "00e8cb49c3ca66a73eac2bf997d449bd83a7801a",
          "message": "Merge pull request #5 from assimalign/fix/benchmarks-ci-first-run\n\nfix(benchmarks): first measuring run failures (history bootstrap, keep-alive close, Python stall)",
          "timestamp": "2026-10-19T10:03:39Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/00e8cb49c3ca66a73eac2bf997d449bd83a7801a"
        },
        "date": 1792416972002,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "DataProtectorBenchmarks.Protect_AspNetCore(PayloadSize=64)",
            "value": 11668.276,
            "range": "± 37.966",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080064\nmean 11685.948 ns"
          },
          {
            "name": "DataProtectorBenchmarks.Protect(PayloadSize=64)",
            "value": 7031.632,
            "range": "± 22.817",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080064\nmean 7039.841 ns"
          },
          {
            "name": "DataProtectorBenchmarks.Protect_AspNetCore(PayloadSize=1024)",
            "value": 13958.605,
            "range": "± 23.891",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080064\nmean 13958.706 ns"
          },
          {
            "name": "DataProtectorBenchmarks.Protect(PayloadSize=1024)",
            "value": 7529.374,
            "range": "± 14.396",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080064\nmean 7533.649 ns"
          },
          {
            "name": "DataProtectorBenchmarks.Unprotect_AspNetCore(PayloadSize=64)",
            "value": 9592.028,
            "range": "± 24.804",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080064\nmean 9600.134 ns"
          },
          {
            "name": "DataProtectorBenchmarks.Unprotect(PayloadSize=64)",
            "value": 5739.933,
            "range": "± 15.177",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080064\nmean 5743.276 ns"
          },
          {
            "name": "DataProtectorBenchmarks.Unprotect_AspNetCore(PayloadSize=1024)",
            "value": 10223.903,
            "range": "± 23.573",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080064\nmean 10227.962 ns"
          },
          {
            "name": "DataProtectorBenchmarks.Unprotect(PayloadSize=1024)",
            "value": 6115.394,
            "range": "± 7.639",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080064\nmean 6116.889 ns"
          }
        ]
      }
    ],
    "libraries/Security - allocations": [
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
        "date": 1792403294661,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "DataProtectorBenchmarks.Protect_AspNetCore(PayloadSize=64) - allocated",
            "value": 1192,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079892"
          },
          {
            "name": "DataProtectorBenchmarks.Protect(PayloadSize=64) - allocated",
            "value": 168,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079892"
          },
          {
            "name": "DataProtectorBenchmarks.Protect_AspNetCore(PayloadSize=1024) - allocated",
            "value": 3112,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079892"
          },
          {
            "name": "DataProtectorBenchmarks.Protect(PayloadSize=1024) - allocated",
            "value": 1128,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079892"
          },
          {
            "name": "DataProtectorBenchmarks.Unprotect_AspNetCore(PayloadSize=64) - allocated",
            "value": 920,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079892"
          },
          {
            "name": "DataProtectorBenchmarks.Unprotect(PayloadSize=64) - allocated",
            "value": 120,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079892"
          },
          {
            "name": "DataProtectorBenchmarks.Unprotect_AspNetCore(PayloadSize=1024) - allocated",
            "value": 1880,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079892"
          },
          {
            "name": "DataProtectorBenchmarks.Unprotect(PayloadSize=1024) - allocated",
            "value": 1080,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079892"
          }
        ]
      }
    ],
    "resources/Database - time": [
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
        "date": 1792403314160,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "KeyValuePointBenchmarks.Get",
            "value": 3355.534,
            "range": "± 47.265",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079893\nmean 3375.076 ns"
          },
          {
            "name": "SqlQueryParserBenchmarks.Parse(Statement=Insert)",
            "value": 9984.405,
            "range": "± 18.892",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079893\nmean 9987.105 ns"
          },
          {
            "name": "SqlQueryParserBenchmarks.Parse(Statement=Join)",
            "value": 34082.083,
            "range": "± 59.604",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079893\nmean 34076.459 ns"
          },
          {
            "name": "SqlQueryParserBenchmarks.Parse(Statement=PointSelect)",
            "value": 9941.856,
            "range": "± 22.241",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079893\nmean 9944.282 ns"
          },
          {
            "name": "SqlPointQueryBenchmarks.SelectById_Sqlite",
            "value": 4718.312,
            "range": "± 17.833",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079893\nmean 4723.9 ns"
          },
          {
            "name": "SqlPointQueryBenchmarks.SelectById",
            "value": 14226.991,
            "range": "± 93.902",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079893\nmean 14240.686 ns"
          }
        ]
      },
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
          "id": "00e8cb49c3ca66a73eac2bf997d449bd83a7801a",
          "message": "Merge pull request #5 from assimalign/fix/benchmarks-ci-first-run\n\nfix(benchmarks): first measuring run failures (history bootstrap, keep-alive close, Python stall)",
          "timestamp": "2026-10-19T10:03:39Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/00e8cb49c3ca66a73eac2bf997d449bd83a7801a"
        },
        "date": 1792416988672,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "KeyValuePointBenchmarks.Get",
            "value": 3413.117,
            "range": "± 30.644",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080066\nmean 3418.303 ns"
          },
          {
            "name": "SqlQueryParserBenchmarks.Parse(Statement=Insert)",
            "value": 9966.064,
            "range": "± 26.608",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080066\nmean 9971.063 ns"
          },
          {
            "name": "SqlQueryParserBenchmarks.Parse(Statement=Join)",
            "value": 33815.651,
            "range": "± 46.838",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080066\nmean 33824.111 ns"
          },
          {
            "name": "SqlQueryParserBenchmarks.Parse(Statement=PointSelect)",
            "value": 9907.866,
            "range": "± 24.329",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080066\nmean 9908.683 ns"
          },
          {
            "name": "SqlPointQueryBenchmarks.SelectById_Sqlite",
            "value": 4787.987,
            "range": "± 25.16",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080066\nmean 4793.618 ns"
          },
          {
            "name": "SqlPointQueryBenchmarks.SelectById",
            "value": 14219.209,
            "range": "± 42.142",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080066\nmean 14210.021 ns"
          }
        ]
      }
    ],
    "resources/Database - allocations": [
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
        "date": 1792403316797,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "KeyValuePointBenchmarks.Get - allocated",
            "value": 2521,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079893"
          },
          {
            "name": "SqlQueryParserBenchmarks.Parse(Statement=Insert) - allocated",
            "value": 4016,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079893"
          },
          {
            "name": "SqlQueryParserBenchmarks.Parse(Statement=Join) - allocated",
            "value": 9512,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079893"
          },
          {
            "name": "SqlQueryParserBenchmarks.Parse(Statement=PointSelect) - allocated",
            "value": 3488,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079893"
          },
          {
            "name": "SqlPointQueryBenchmarks.SelectById_Sqlite - allocated",
            "value": 1040,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079893"
          },
          {
            "name": "SqlPointQueryBenchmarks.SelectById - allocated",
            "value": 14150,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079893"
          }
        ]
      },
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
          "id": "00e8cb49c3ca66a73eac2bf997d449bd83a7801a",
          "message": "Merge pull request #5 from assimalign/fix/benchmarks-ci-first-run\n\nfix(benchmarks): first measuring run failures (history bootstrap, keep-alive close, Python stall)",
          "timestamp": "2026-10-19T10:03:39Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/00e8cb49c3ca66a73eac2bf997d449bd83a7801a"
        },
        "date": 1792416991219,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "KeyValuePointBenchmarks.Get - allocated",
            "value": 2521,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080066"
          },
          {
            "name": "SqlQueryParserBenchmarks.Parse(Statement=Insert) - allocated",
            "value": 4016,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080066"
          },
          {
            "name": "SqlQueryParserBenchmarks.Parse(Statement=Join) - allocated",
            "value": 9512,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080066"
          },
          {
            "name": "SqlQueryParserBenchmarks.Parse(Statement=PointSelect) - allocated",
            "value": 3488,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080066"
          },
          {
            "name": "SqlPointQueryBenchmarks.SelectById_Sqlite - allocated",
            "value": 1040,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080066"
          },
          {
            "name": "SqlPointQueryBenchmarks.SelectById - allocated",
            "value": 14150,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080066"
          }
        ]
      }
    ],
    "resources/NatGateway - time": [
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
        "date": 1792403335671,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "NatGatewayApplicationBenchmarks.BuildStartStop(ServiceCount=1)",
            "value": 4384.404,
            "range": "± 10.382",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079894\nmean 4384.942 ns"
          },
          {
            "name": "NatGatewayApplicationBenchmarks.BuildStartStop(ServiceCount=16)",
            "value": 12653.914,
            "range": "± 37.929",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079894\nmean 12639.157 ns"
          }
        ]
      },
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
          "id": "00e8cb49c3ca66a73eac2bf997d449bd83a7801a",
          "message": "Merge pull request #5 from assimalign/fix/benchmarks-ci-first-run\n\nfix(benchmarks): first measuring run failures (history bootstrap, keep-alive close, Python stall)",
          "timestamp": "2026-10-19T10:03:39Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/00e8cb49c3ca66a73eac2bf997d449bd83a7801a"
        },
        "date": 1792417007408,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "NatGatewayApplicationBenchmarks.BuildStartStop(ServiceCount=1)",
            "value": 4985.321,
            "range": "± 35.456",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080067\nmean 4985.978 ns"
          },
          {
            "name": "NatGatewayApplicationBenchmarks.BuildStartStop(ServiceCount=16)",
            "value": 12864.211,
            "range": "± 88.201",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080067\nmean 12870.891 ns"
          }
        ]
      }
    ],
    "resources/NatGateway - allocations": [
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
        "date": 1792403338322,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "NatGatewayApplicationBenchmarks.BuildStartStop(ServiceCount=1) - allocated",
            "value": 7856,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079894"
          },
          {
            "name": "NatGatewayApplicationBenchmarks.BuildStartStop(ServiceCount=16) - allocated",
            "value": 20944,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079894"
          }
        ]
      },
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
          "id": "00e8cb49c3ca66a73eac2bf997d449bd83a7801a",
          "message": "Merge pull request #5 from assimalign/fix/benchmarks-ci-first-run\n\nfix(benchmarks): first measuring run failures (history bootstrap, keep-alive close, Python stall)",
          "timestamp": "2026-10-19T10:03:39Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/00e8cb49c3ca66a73eac2bf997d449bd83a7801a"
        },
        "date": 1792417010192,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "NatGatewayApplicationBenchmarks.BuildStartStop(ServiceCount=1) - allocated",
            "value": 7856,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080067"
          },
          {
            "name": "NatGatewayApplicationBenchmarks.BuildStartStop(ServiceCount=16) - allocated",
            "value": 20944,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080067"
          }
        ]
      }
    ],
    "resources/Scheduler - time": [
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
        "date": 1792403352732,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "CrontabBenchmarks.NextOccurrence_Cronos(Expression=*/5 * * * *)",
            "value": 31.585,
            "range": "± 0.062",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079895\nmean 31.599 ns"
          },
          {
            "name": "CrontabBenchmarks.NextOccurrence(Expression=*/5 * * * *)",
            "value": 249.408,
            "range": "± 0.859",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079895\nmean 249.56 ns"
          },
          {
            "name": "CrontabBenchmarks.NextOccurrence_Cronos(Expression=0 9 * * 1-5)",
            "value": 71.256,
            "range": "± 0.099",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079895\nmean 71.27 ns"
          },
          {
            "name": "CrontabBenchmarks.NextOccurrence(Expression=0 9 * * 1-5)",
            "value": 1220.49,
            "range": "± 2.881",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079895\nmean 1221.223 ns"
          },
          {
            "name": "CrontabBenchmarks.Parse_Cronos(Expression=*/5 * * * *)",
            "value": 40.974,
            "range": "± 0.189",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079895\nmean 40.964 ns"
          },
          {
            "name": "CrontabBenchmarks.Parse(Expression=*/5 * * * *)",
            "value": 3767.112,
            "range": "± 51.187",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079895\nmean 3748.974 ns"
          },
          {
            "name": "CrontabBenchmarks.Parse_Cronos(Expression=0 9 * * 1-5)",
            "value": 57.222,
            "range": "± 0.145",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079895\nmean 57.178 ns"
          },
          {
            "name": "CrontabBenchmarks.Parse(Expression=0 9 * * 1-5)",
            "value": 2469.05,
            "range": "± 111.449",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079895\nmean 2476.232 ns"
          }
        ]
      },
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
          "id": "00e8cb49c3ca66a73eac2bf997d449bd83a7801a",
          "message": "Merge pull request #5 from assimalign/fix/benchmarks-ci-first-run\n\nfix(benchmarks): first measuring run failures (history bootstrap, keep-alive close, Python stall)",
          "timestamp": "2026-10-19T10:03:39Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/00e8cb49c3ca66a73eac2bf997d449bd83a7801a"
        },
        "date": 1792417021646,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "CrontabBenchmarks.NextOccurrence_Cronos(Expression=*/5 * * * *)",
            "value": 32.08,
            "range": "± 0.128",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080069\nmean 32.133 ns"
          },
          {
            "name": "CrontabBenchmarks.NextOccurrence(Expression=*/5 * * * *)",
            "value": 251.218,
            "range": "± 1.067",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080069\nmean 251.086 ns"
          },
          {
            "name": "CrontabBenchmarks.NextOccurrence_Cronos(Expression=0 9 * * 1-5)",
            "value": 71.087,
            "range": "± 0.051",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080069\nmean 71.088 ns"
          },
          {
            "name": "CrontabBenchmarks.NextOccurrence(Expression=0 9 * * 1-5)",
            "value": 1214.338,
            "range": "± 1.652",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080069\nmean 1214.412 ns"
          },
          {
            "name": "CrontabBenchmarks.Parse_Cronos(Expression=*/5 * * * *)",
            "value": 39.204,
            "range": "± 0.368",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080069\nmean 39.343 ns"
          },
          {
            "name": "CrontabBenchmarks.Parse(Expression=*/5 * * * *)",
            "value": 3589.965,
            "range": "± 25.536",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080069\nmean 3588.042 ns"
          },
          {
            "name": "CrontabBenchmarks.Parse_Cronos(Expression=0 9 * * 1-5)",
            "value": 56.221,
            "range": "± 0.149",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080069\nmean 56.187 ns"
          },
          {
            "name": "CrontabBenchmarks.Parse(Expression=0 9 * * 1-5)",
            "value": 2477.112,
            "range": "± 19.907",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080069\nmean 2477.766 ns"
          }
        ]
      }
    ],
    "resources/Scheduler - allocations": [
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
        "date": 1792403354544,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "CrontabBenchmarks.NextOccurrence_Cronos(Expression=*/5 * * * *) - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079895"
          },
          {
            "name": "CrontabBenchmarks.NextOccurrence(Expression=*/5 * * * *) - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079895"
          },
          {
            "name": "CrontabBenchmarks.NextOccurrence_Cronos(Expression=0 9 * * 1-5) - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079895"
          },
          {
            "name": "CrontabBenchmarks.NextOccurrence(Expression=0 9 * * 1-5) - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079895"
          },
          {
            "name": "CrontabBenchmarks.Parse_Cronos(Expression=*/5 * * * *) - allocated",
            "value": 48,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079895"
          },
          {
            "name": "CrontabBenchmarks.Parse(Expression=*/5 * * * *) - allocated",
            "value": 6272,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079895"
          },
          {
            "name": "CrontabBenchmarks.Parse_Cronos(Expression=0 9 * * 1-5) - allocated",
            "value": 48,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079895"
          },
          {
            "name": "CrontabBenchmarks.Parse(Expression=0 9 * * 1-5) - allocated",
            "value": 4656,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079895"
          }
        ]
      },
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
          "id": "00e8cb49c3ca66a73eac2bf997d449bd83a7801a",
          "message": "Merge pull request #5 from assimalign/fix/benchmarks-ci-first-run\n\nfix(benchmarks): first measuring run failures (history bootstrap, keep-alive close, Python stall)",
          "timestamp": "2026-10-19T10:03:39Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/00e8cb49c3ca66a73eac2bf997d449bd83a7801a"
        },
        "date": 1792417023705,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "CrontabBenchmarks.NextOccurrence_Cronos(Expression=*/5 * * * *) - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080069"
          },
          {
            "name": "CrontabBenchmarks.NextOccurrence(Expression=*/5 * * * *) - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080069"
          },
          {
            "name": "CrontabBenchmarks.NextOccurrence_Cronos(Expression=0 9 * * 1-5) - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080069"
          },
          {
            "name": "CrontabBenchmarks.NextOccurrence(Expression=0 9 * * 1-5) - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080069"
          },
          {
            "name": "CrontabBenchmarks.Parse_Cronos(Expression=*/5 * * * *) - allocated",
            "value": 48,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080069"
          },
          {
            "name": "CrontabBenchmarks.Parse(Expression=*/5 * * * *) - allocated",
            "value": 6272,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080069"
          },
          {
            "name": "CrontabBenchmarks.Parse_Cronos(Expression=0 9 * * 1-5) - allocated",
            "value": 48,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080069"
          },
          {
            "name": "CrontabBenchmarks.Parse(Expression=0 9 * * 1-5) - allocated",
            "value": 4656,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080069"
          }
        ]
      }
    ],
    "resources/Web - time": [
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
        "date": 1792403365930,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "HttpRoundTripBenchmarks.Get(Target=node)",
            "value": 34368.711,
            "range": "± 4781.608",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079896\nmean 34651.015 ns"
          },
          {
            "name": "HttpRoundTripBenchmarks.Get(Target=python)",
            "value": 40998518,
            "range": "± 26242.837",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079896\nmean 41001480.709 ns"
          },
          {
            "name": "ProcessStartupBenchmarks.StartUntilReady(server=node)",
            "value": 67792887,
            "range": "± 1367564.69",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079896\nmean 68181633.8 ns"
          },
          {
            "name": "ProcessStartupBenchmarks.RunToExit(command=node-run)",
            "value": 17812676,
            "range": "± 1147382.948",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079896\nmean 17727810.3 ns"
          },
          {
            "name": "ProcessStartupBenchmarks.StartUntilReady(server=python)",
            "value": 65451542,
            "range": "± 2221235.743",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079896\nmean 66433134.45 ns"
          },
          {
            "name": "JwtBearerAuthenticationBenchmarks.Authenticate_AspNetCore",
            "value": 5786.547,
            "range": "± 109.831",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079896\nmean 5815.321 ns"
          },
          {
            "name": "JwtBearerAuthenticationBenchmarks.Authenticate",
            "value": 5698.045,
            "range": "± 255.154",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079896\nmean 5616.152 ns"
          },
          {
            "name": "CookieAuthenticationBenchmarks.Authenticate_AspNetCore",
            "value": 9920.838,
            "range": "± 43.798",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079896\nmean 9938.329 ns"
          },
          {
            "name": "CookieAuthenticationBenchmarks.Authenticate",
            "value": 5993.022,
            "range": "± 41.438",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079896\nmean 5987.887 ns"
          },
          {
            "name": "RolePolicyBenchmarks.Evaluate_AspNetCore",
            "value": 300.623,
            "range": "± 14.57",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079896\nmean 304.66 ns"
          },
          {
            "name": "RolePolicyBenchmarks.Evaluate",
            "value": 38.95,
            "range": "± 1.881",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079896\nmean 39.571 ns"
          }
        ]
      },
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
          "id": "00e8cb49c3ca66a73eac2bf997d449bd83a7801a",
          "message": "Merge pull request #5 from assimalign/fix/benchmarks-ci-first-run\n\nfix(benchmarks): first measuring run failures (history bootstrap, keep-alive close, Python stall)",
          "timestamp": "2026-10-19T10:03:39Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/00e8cb49c3ca66a73eac2bf997d449bd83a7801a"
        },
        "date": 1792417038426,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "HttpRoundTripBenchmarks.Get(Target=nginx)",
            "value": 192828.758,
            "range": "± 981.234",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080070\nmean 193211.119 ns"
          },
          {
            "name": "HttpRoundTripBenchmarks.Get(Target=node)",
            "value": 68489.579,
            "range": "± 6811.814",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080070\nmean 67492.408 ns"
          },
          {
            "name": "HttpRoundTripBenchmarks.Get(Target=python)",
            "value": 109903.699,
            "range": "± 7165.028",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080070\nmean 110904.218 ns"
          },
          {
            "name": "ProcessStartupBenchmarks.StartUntilReady(server=node)",
            "value": 86661256,
            "range": "± 3839796.134",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080070\nmean 88303980.35 ns"
          },
          {
            "name": "ProcessStartupBenchmarks.RunToExit(command=node-run)",
            "value": 22707041.5,
            "range": "± 867860.816",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080070\nmean 22689822.35 ns"
          },
          {
            "name": "ProcessStartupBenchmarks.StartUntilReady(server=python)",
            "value": 79949563,
            "range": "± 4240336.21",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080070\nmean 81847159.15 ns"
          },
          {
            "name": "JwtBearerAuthenticationBenchmarks.Authenticate_AspNetCore",
            "value": 7702.891,
            "range": "± 36.353",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080070\nmean 7714.844 ns"
          },
          {
            "name": "JwtBearerAuthenticationBenchmarks.Authenticate",
            "value": 6693.832,
            "range": "± 49.228",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080070\nmean 6704.932 ns"
          },
          {
            "name": "CookieAuthenticationBenchmarks.Authenticate_AspNetCore",
            "value": 15646.721,
            "range": "± 62.439",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080070\nmean 15657.243 ns"
          },
          {
            "name": "CookieAuthenticationBenchmarks.Authenticate",
            "value": 8207.078,
            "range": "± 20.482",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080070\nmean 8206.731 ns"
          },
          {
            "name": "RolePolicyBenchmarks.Evaluate_AspNetCore",
            "value": 417.052,
            "range": "± 1.897",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080070\nmean 417.344 ns"
          },
          {
            "name": "RolePolicyBenchmarks.Evaluate",
            "value": 75.291,
            "range": "± 0.127",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080070\nmean 75.304 ns"
          }
        ]
      }
    ],
    "resources/Web - allocations": [
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
        "date": 1792403367706,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "HttpRoundTripBenchmarks.Get(Target=node) - allocated",
            "value": 64,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079896"
          },
          {
            "name": "HttpRoundTripBenchmarks.Get(Target=python) - allocated",
            "value": 377,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079896"
          },
          {
            "name": "ProcessStartupBenchmarks.StartUntilReady(server=node) - allocated",
            "value": 2330072,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079896"
          },
          {
            "name": "ProcessStartupBenchmarks.RunToExit(command=node-run) - allocated",
            "value": 95920,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079896"
          },
          {
            "name": "ProcessStartupBenchmarks.StartUntilReady(server=python) - allocated",
            "value": 2317120,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079896"
          },
          {
            "name": "JwtBearerAuthenticationBenchmarks.Authenticate_AspNetCore - allocated",
            "value": 12256,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079896"
          },
          {
            "name": "JwtBearerAuthenticationBenchmarks.Authenticate - allocated",
            "value": 10712,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079896"
          },
          {
            "name": "CookieAuthenticationBenchmarks.Authenticate_AspNetCore - allocated",
            "value": 7744,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079896"
          },
          {
            "name": "CookieAuthenticationBenchmarks.Authenticate - allocated",
            "value": 5936,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079896"
          },
          {
            "name": "RolePolicyBenchmarks.Evaluate_AspNetCore - allocated",
            "value": 704,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079896"
          },
          {
            "name": "RolePolicyBenchmarks.Evaluate - allocated",
            "value": 72,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000079896"
          }
        ]
      }
    ],
    "libraries/Amqp - time": [
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
          "id": "00e8cb49c3ca66a73eac2bf997d449bd83a7801a",
          "message": "Merge pull request #5 from assimalign/fix/benchmarks-ci-first-run\n\nfix(benchmarks): first measuring run failures (history bootstrap, keep-alive close, Python stall)",
          "timestamp": "2026-10-19T10:03:39Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/00e8cb49c3ca66a73eac2bf997d449bd83a7801a"
        },
        "date": 1792416609586,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "AmqpFrameBenchmarks.EncodeTransferFrame",
            "value": 222.354,
            "range": "± 2.862",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080029\nmean 222.706 ns"
          },
          {
            "name": "AmqpFrameBenchmarks.DecodeTransferFrame",
            "value": 81.678,
            "range": "± 0.617",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080029\nmean 81.513 ns"
          },
          {
            "name": "AmqpMessageBenchmarks.Decode_AmqpNetLite",
            "value": 254.132,
            "range": "± 2.456",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080029\nmean 254.624 ns"
          },
          {
            "name": "AmqpMessageBenchmarks.Decode",
            "value": 373.576,
            "range": "± 2.384",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080029\nmean 373.694 ns"
          },
          {
            "name": "AmqpMessageBenchmarks.Encode_AmqpNetLite",
            "value": 309.692,
            "range": "± 4.907",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080029\nmean 310.829 ns"
          },
          {
            "name": "AmqpMessageBenchmarks.Encode",
            "value": 380.001,
            "range": "± 5.083",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080029\nmean 381.684 ns"
          }
        ]
      },
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
          "id": "06c9dcd86539ac3365bc0bb6b3432db5ad769ce5",
          "message": "Merge pull request #7 from assimalign/fix/record-both-metrics-on-alert\n\nRecord allocations even when the time series alerts",
          "timestamp": "2026-10-19T20:26:47Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/06c9dcd86539ac3365bc0bb6b3432db5ad769ce5"
        },
        "date": 1792448634226,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "AmqpFrameBenchmarks.EncodeTransferFrame",
            "value": 492.277,
            "range": "± 12.042",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080315\nmean 491.481 ns"
          },
          {
            "name": "AmqpFrameBenchmarks.DecodeTransferFrame",
            "value": 181.027,
            "range": "± 4.264",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080315\nmean 180.41 ns"
          },
          {
            "name": "AmqpMessageBenchmarks.Decode_AmqpNetLite",
            "value": 548.059,
            "range": "± 5.932",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080315\nmean 547.703 ns"
          },
          {
            "name": "AmqpMessageBenchmarks.Decode",
            "value": 811.728,
            "range": "± 6.562",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080315\nmean 810.499 ns"
          },
          {
            "name": "AmqpMessageBenchmarks.Encode_AmqpNetLite",
            "value": 666.152,
            "range": "± 4.052",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080315\nmean 665.917 ns"
          },
          {
            "name": "AmqpMessageBenchmarks.Encode",
            "value": 893.018,
            "range": "± 14.157",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080315\nmean 892.136 ns"
          }
        ]
      }
    ],
    "libraries/Amqp - allocations": [
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
          "id": "00e8cb49c3ca66a73eac2bf997d449bd83a7801a",
          "message": "Merge pull request #5 from assimalign/fix/benchmarks-ci-first-run\n\nfix(benchmarks): first measuring run failures (history bootstrap, keep-alive close, Python stall)",
          "timestamp": "2026-10-19T10:03:39Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/00e8cb49c3ca66a73eac2bf997d449bd83a7801a"
        },
        "date": 1792416612124,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "AmqpFrameBenchmarks.EncodeTransferFrame - allocated",
            "value": 3352,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080029"
          },
          {
            "name": "AmqpFrameBenchmarks.DecodeTransferFrame - allocated",
            "value": 888,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080029"
          },
          {
            "name": "AmqpMessageBenchmarks.Decode_AmqpNetLite - allocated",
            "value": 1184,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080029"
          },
          {
            "name": "AmqpMessageBenchmarks.Decode - allocated",
            "value": 1856,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080029"
          },
          {
            "name": "AmqpMessageBenchmarks.Encode_AmqpNetLite - allocated",
            "value": 1056,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080029"
          },
          {
            "name": "AmqpMessageBenchmarks.Encode - allocated",
            "value": 2680,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.38 (ff7a93e80a9a) on GitHub Actions 1000080029"
          }
        ]
      },
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
          "id": "06c9dcd86539ac3365bc0bb6b3432db5ad769ce5",
          "message": "Merge pull request #7 from assimalign/fix/record-both-metrics-on-alert\n\nRecord allocations even when the time series alerts",
          "timestamp": "2026-10-19T20:26:47Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/06c9dcd86539ac3365bc0bb6b3432db5ad769ce5"
        },
        "date": 1792448637127,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "AmqpFrameBenchmarks.EncodeTransferFrame - allocated",
            "value": 3352,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080315"
          },
          {
            "name": "AmqpFrameBenchmarks.DecodeTransferFrame - allocated",
            "value": 888,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080315"
          },
          {
            "name": "AmqpMessageBenchmarks.Decode_AmqpNetLite - allocated",
            "value": 1184,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080315"
          },
          {
            "name": "AmqpMessageBenchmarks.Decode - allocated",
            "value": 1856,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080315"
          },
          {
            "name": "AmqpMessageBenchmarks.Encode_AmqpNetLite - allocated",
            "value": 1056,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080315"
          },
          {
            "name": "AmqpMessageBenchmarks.Encode - allocated",
            "value": 2680,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080315"
          }
        ]
      }
    ]
  }
}