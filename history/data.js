window.BENCHMARK_DATA = {
  "lastUpdate": 1792485139625,
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
          "id": "699980c0cf28b76854e64dc51e4e45d2c5c05dee",
          "message": "Merge pull request #8 from assimalign/feature/benchmark-suite-expansion\n\nBuild out the benchmark suites: 148 to 988 cases across every area but Database",
          "timestamp": "2026-10-20T07:59:35Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/699980c0cf28b76854e64dc51e4e45d2c5c05dee"
        },
        "date": 1792485044821,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "ApplicationGraphBenchmarks.ParseManifests(ResourceCount=4)",
            "value": 28263.425,
            "range": "± 174.593",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080624\nmean 28305.228 ns"
          },
          {
            "name": "ApplicationGraphBenchmarks.ValidateManifests(ResourceCount=4)",
            "value": 1627.418,
            "range": "± 24.985",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080624\nmean 1632.997 ns"
          },
          {
            "name": "ApplicationGraphBenchmarks.CreatePlans(ResourceCount=4)",
            "value": 3688.177,
            "range": "± 66.49",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080624\nmean 3683.225 ns"
          },
          {
            "name": "ApplicationGraphBenchmarks.ValidatePlans(ResourceCount=4)",
            "value": 15794.798,
            "range": "± 234.071",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080624\nmean 15776.982 ns"
          },
          {
            "name": "ApplicationGraphBenchmarks.Build(ResourceCount=4)",
            "value": 30280.918,
            "range": "± 393.682",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080624\nmean 30356.853 ns"
          },
          {
            "name": "ApplicationGraphBenchmarks.ComputeManifestHashes(ResourceCount=4)",
            "value": 86160.794,
            "range": "± 709.75",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080624\nmean 86099.679 ns"
          },
          {
            "name": "ApplicationGraphBenchmarks.ParseManifests(ResourceCount=24)",
            "value": 192897.655,
            "range": "± 1085.822",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080624\nmean 192459.736 ns"
          },
          {
            "name": "ApplicationGraphBenchmarks.ValidateManifests(ResourceCount=24)",
            "value": 11980.116,
            "range": "± 106.383",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080624\nmean 11984.294 ns"
          },
          {
            "name": "ApplicationGraphBenchmarks.CreatePlans(ResourceCount=24)",
            "value": 22413.477,
            "range": "± 444.385",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080624\nmean 22433.135 ns"
          },
          {
            "name": "ApplicationGraphBenchmarks.ValidatePlans(ResourceCount=24)",
            "value": 101754.036,
            "range": "± 2055.25",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080624\nmean 102154.344 ns"
          },
          {
            "name": "ApplicationGraphBenchmarks.Build(ResourceCount=24)",
            "value": 197661.61,
            "range": "± 2408.324",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080624\nmean 197336.778 ns"
          },
          {
            "name": "ApplicationGraphBenchmarks.ComputeManifestHashes(ResourceCount=24)",
            "value": 552459.492,
            "range": "± 9400.82",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080624\nmean 550073.324 ns"
          },
          {
            "name": "ApplicationModelDocumentBenchmarks.CreateDocument(ResourceCount=4)",
            "value": 2112.478,
            "range": "± 38.031",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080624\nmean 2104.333 ns"
          },
          {
            "name": "ApplicationModelDocumentBenchmarks.ParseDocument(ResourceCount=4)",
            "value": 103386.648,
            "range": "± 1002.457",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080624\nmean 103292.207 ns"
          },
          {
            "name": "ApplicationModelDocumentBenchmarks.ReconstructModel(ResourceCount=4)",
            "value": 23318.719,
            "range": "± 419.751",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080624\nmean 23326.652 ns"
          },
          {
            "name": "ApplicationModelDocumentBenchmarks.CreateDocument(ResourceCount=24)",
            "value": 14954.102,
            "range": "± 301.747",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080624\nmean 14923.081 ns"
          },
          {
            "name": "ApplicationModelDocumentBenchmarks.ParseDocument(ResourceCount=24)",
            "value": 670964.706,
            "range": "± 3450.724",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080624\nmean 672013.29 ns"
          },
          {
            "name": "ApplicationModelDocumentBenchmarks.ReconstructModel(ResourceCount=24)",
            "value": 151923.561,
            "range": "± 1551.757",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080624\nmean 152064.344 ns"
          },
          {
            "name": "ApplicationSetBenchmarks.ComposeApplications(ResourcesPerApplication=4)",
            "value": 1526.445,
            "range": "± 20.796",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080624\nmean 1531.862 ns"
          },
          {
            "name": "ApplicationSetBenchmarks.ComposeApplications(ResourcesPerApplication=24)",
            "value": 7560.713,
            "range": "± 74.555",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080624\nmean 7584.144 ns"
          },
          {
            "name": "GenericPlannerBenchmarks.CreatePlan",
            "value": 1033.028,
            "range": "± 23.58",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080624\nmean 1035.357 ns"
          },
          {
            "name": "GenericPlannerBenchmarks.ValidatePlan",
            "value": 4718.277,
            "range": "± 117.993",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080624\nmean 4731.171 ns"
          },
          {
            "name": "ResourceManifestBenchmarks.Parse",
            "value": 6767.136,
            "range": "± 106.299",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080624\nmean 6772.779 ns"
          },
          {
            "name": "ResourceManifestBenchmarks.ComputeHash",
            "value": 20389.592,
            "range": "± 265.662",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080624\nmean 20400.421 ns"
          },
          {
            "name": "DirectCommandBenchmarks.ReconcileRepeatedCommands(ResourceCount=4)",
            "value": 505744.906,
            "range": "± 9552.268",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080624\nmean 505801.682 ns"
          },
          {
            "name": "DirectCommandBenchmarks.ReplaceCommands(ResourceCount=4)",
            "value": 994796.038,
            "range": "± 15067.053",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080624\nmean 998425.286 ns"
          },
          {
            "name": "DirectCommandBenchmarks.ReconcileRepeatedCommands(ResourceCount=24)",
            "value": 3372721.762,
            "range": "± 37959.957",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080624\nmean 3379260.818 ns"
          },
          {
            "name": "DirectCommandBenchmarks.ReplaceCommands(ResourceCount=24)",
            "value": 6185761.363,
            "range": "± 117583.989",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080624\nmean 6159791.083 ns"
          },
          {
            "name": "ResourceResolutionBenchmarks.ValidateGraph(ResourceCount=4)",
            "value": 323.757,
            "range": "± 3.158",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080624\nmean 323.218 ns"
          },
          {
            "name": "ResourceResolutionBenchmarks.ResolveMountSources(ResourceCount=4)",
            "value": 12700.573,
            "range": "± 145.032",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080624\nmean 12701.716 ns"
          },
          {
            "name": "ResourceResolutionBenchmarks.ValidateGraph(ResourceCount=24)",
            "value": 1478.956,
            "range": "± 16.171",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080624\nmean 1484.939 ns"
          },
          {
            "name": "ResourceResolutionBenchmarks.ResolveMountSources(ResourceCount=24)",
            "value": 97931.341,
            "range": "± 1988.079",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080624\nmean 97911.136 ns"
          },
          {
            "name": "ControlPlaneRoutingBenchmarks.MatchApplication",
            "value": 109.719,
            "range": "± 1.062",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080624\nmean 109.727 ns"
          },
          {
            "name": "ControlPlaneRoutingBenchmarks.MatchCommand",
            "value": 224.742,
            "range": "± 3.296",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080624\nmean 224.846 ns"
          },
          {
            "name": "ControlPlaneRoutingBenchmarks.MatchCommandDeletion",
            "value": 347.493,
            "range": "± 5.784",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080624\nmean 347.143 ns"
          },
          {
            "name": "ControlPlaneRoutingBenchmarks.MatchHead",
            "value": 114.485,
            "range": "± 1.233",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080624\nmean 114.322 ns"
          },
          {
            "name": "ControlPlaneRoutingBenchmarks.RejectMethod",
            "value": 374.74,
            "range": "± 5.284",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080624\nmean 373.299 ns"
          },
          {
            "name": "ControlPlaneRoutingBenchmarks.MissPath",
            "value": 117.013,
            "range": "± 1.56",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080624\nmean 116.66 ns"
          },
          {
            "name": "InProcessPlanBenchmarks.ValidateBindings(ResourceCount=4)",
            "value": 964.082,
            "range": "± 18.224",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080624\nmean 961.104 ns"
          },
          {
            "name": "InProcessPlanBenchmarks.RenderPlan(ResourceCount=4)",
            "value": 21136.99,
            "range": "± 795.388",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080624\nmean 21300.842 ns"
          },
          {
            "name": "InProcessPlanBenchmarks.ValidateBindings(ResourceCount=24)",
            "value": 5468.79,
            "range": "± 44.688",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080624\nmean 5465.208 ns"
          },
          {
            "name": "InProcessPlanBenchmarks.RenderPlan(ResourceCount=24)",
            "value": 443968.116,
            "range": "± 11442.008",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080624\nmean 440314.34 ns"
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
          "id": "699980c0cf28b76854e64dc51e4e45d2c5c05dee",
          "message": "Merge pull request #8 from assimalign/feature/benchmark-suite-expansion\n\nBuild out the benchmark suites: 148 to 988 cases across every area but Database",
          "timestamp": "2026-10-20T07:59:35Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/699980c0cf28b76854e64dc51e4e45d2c5c05dee"
        },
        "date": 1792485047270,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "ApplicationGraphBenchmarks.ParseManifests(ResourceCount=4) - allocated",
            "value": 22657,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080624"
          },
          {
            "name": "ApplicationGraphBenchmarks.ValidateManifests(ResourceCount=4) - allocated",
            "value": 3720,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080624"
          },
          {
            "name": "ApplicationGraphBenchmarks.CreatePlans(ResourceCount=4) - allocated",
            "value": 13200,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080624"
          },
          {
            "name": "ApplicationGraphBenchmarks.ValidatePlans(ResourceCount=4) - allocated",
            "value": 48507,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080624"
          },
          {
            "name": "ApplicationGraphBenchmarks.Build(ResourceCount=4) - allocated",
            "value": 82342,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080624"
          },
          {
            "name": "ApplicationGraphBenchmarks.ComputeManifestHashes(ResourceCount=4) - allocated",
            "value": 135533,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080624"
          },
          {
            "name": "ApplicationGraphBenchmarks.ParseManifests(ResourceCount=24) - allocated",
            "value": 155782,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080624"
          },
          {
            "name": "ApplicationGraphBenchmarks.ValidateManifests(ResourceCount=24) - allocated",
            "value": 25641,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080624"
          },
          {
            "name": "ApplicationGraphBenchmarks.CreatePlans(ResourceCount=24) - allocated",
            "value": 81200,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080624"
          },
          {
            "name": "ApplicationGraphBenchmarks.ValidatePlans(ResourceCount=24) - allocated",
            "value": 307209,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080624"
          },
          {
            "name": "ApplicationGraphBenchmarks.Build(ResourceCount=24) - allocated",
            "value": 510725,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080624"
          },
          {
            "name": "ApplicationGraphBenchmarks.ComputeManifestHashes(ResourceCount=24) - allocated",
            "value": 893319,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080624"
          },
          {
            "name": "ApplicationModelDocumentBenchmarks.CreateDocument(ResourceCount=4) - allocated",
            "value": 7184,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080624"
          },
          {
            "name": "ApplicationModelDocumentBenchmarks.ParseDocument(ResourceCount=4) - allocated",
            "value": 111212,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080624"
          },
          {
            "name": "ApplicationModelDocumentBenchmarks.ReconstructModel(ResourceCount=4) - allocated",
            "value": 60630,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080624"
          },
          {
            "name": "ApplicationModelDocumentBenchmarks.CreateDocument(ResourceCount=24) - allocated",
            "value": 47346,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080624"
          },
          {
            "name": "ApplicationModelDocumentBenchmarks.ParseDocument(ResourceCount=24) - allocated",
            "value": 708495,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080624"
          },
          {
            "name": "ApplicationModelDocumentBenchmarks.ReconstructModel(ResourceCount=24) - allocated",
            "value": 383528,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080624"
          },
          {
            "name": "ApplicationSetBenchmarks.ComposeApplications(ResourcesPerApplication=4) - allocated",
            "value": 3440,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080624"
          },
          {
            "name": "ApplicationSetBenchmarks.ComposeApplications(ResourcesPerApplication=24) - allocated",
            "value": 12720,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080624"
          },
          {
            "name": "GenericPlannerBenchmarks.CreatePlan - allocated",
            "value": 3512,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080624"
          },
          {
            "name": "GenericPlannerBenchmarks.ValidatePlan - allocated",
            "value": 14353,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080624"
          },
          {
            "name": "ResourceManifestBenchmarks.Parse - allocated",
            "value": 5208,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080624"
          },
          {
            "name": "ResourceManifestBenchmarks.ComputeHash - allocated",
            "value": 33057,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080624"
          },
          {
            "name": "DirectCommandBenchmarks.ReconcileRepeatedCommands(ResourceCount=4) - allocated",
            "value": 420546,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080624"
          },
          {
            "name": "DirectCommandBenchmarks.ReplaceCommands(ResourceCount=4) - allocated",
            "value": 789782,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080624"
          },
          {
            "name": "DirectCommandBenchmarks.ReconcileRepeatedCommands(ResourceCount=24) - allocated",
            "value": 2918057,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080624"
          },
          {
            "name": "DirectCommandBenchmarks.ReplaceCommands(ResourceCount=24) - allocated",
            "value": 5813189,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080624"
          },
          {
            "name": "ResourceResolutionBenchmarks.ValidateGraph(ResourceCount=4) - allocated",
            "value": 592,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080624"
          },
          {
            "name": "ResourceResolutionBenchmarks.ResolveMountSources(ResourceCount=4) - allocated",
            "value": 15352,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080624"
          },
          {
            "name": "ResourceResolutionBenchmarks.ValidateGraph(ResourceCount=24) - allocated",
            "value": 2192,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080624"
          },
          {
            "name": "ResourceResolutionBenchmarks.ResolveMountSources(ResourceCount=24) - allocated",
            "value": 116473,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080624"
          },
          {
            "name": "ControlPlaneRoutingBenchmarks.MatchApplication - allocated",
            "value": 240,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080624"
          },
          {
            "name": "ControlPlaneRoutingBenchmarks.MatchCommand - allocated",
            "value": 688,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080624"
          },
          {
            "name": "ControlPlaneRoutingBenchmarks.MatchCommandDeletion - allocated",
            "value": 992,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080624"
          },
          {
            "name": "ControlPlaneRoutingBenchmarks.MatchHead - allocated",
            "value": 240,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080624"
          },
          {
            "name": "ControlPlaneRoutingBenchmarks.RejectMethod - allocated",
            "value": 992,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080624"
          },
          {
            "name": "ControlPlaneRoutingBenchmarks.MissPath - allocated",
            "value": 304,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080624"
          },
          {
            "name": "InProcessPlanBenchmarks.ValidateBindings(ResourceCount=4) - allocated",
            "value": 1232,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080624"
          },
          {
            "name": "InProcessPlanBenchmarks.RenderPlan(ResourceCount=4) - allocated",
            "value": 114432,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080624"
          },
          {
            "name": "InProcessPlanBenchmarks.ValidateBindings(ResourceCount=24) - allocated",
            "value": 6032,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080624"
          },
          {
            "name": "InProcessPlanBenchmarks.RenderPlan(ResourceCount=24) - allocated",
            "value": 730527,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080624"
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
          "id": "699980c0cf28b76854e64dc51e4e45d2c5c05dee",
          "message": "Merge pull request #8 from assimalign/feature/benchmark-suite-expansion\n\nBuild out the benchmark suites: 148 to 988 cases across every area but Database",
          "timestamp": "2026-10-20T07:59:35Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/699980c0cf28b76854e64dc51e4e45d2c5c05dee"
        },
        "date": 1792485057908,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "GetOrCreateBenchmarks.Create",
            "value": 6121,
            "range": "± 128.883",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080628\nmean 6131 ns"
          },
          {
            "name": "GetOrCreateBenchmarks.Create_MicrosoftExtensions",
            "value": 4498,
            "range": "± 61.407",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080628\nmean 4504.15 ns"
          },
          {
            "name": "GetOrCreateHitBenchmarks.Hit",
            "value": 72.438,
            "range": "± 0.145",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080628\nmean 72.451 ns"
          },
          {
            "name": "GetOrCreateHitBenchmarks.Hit_MicrosoftExtensions",
            "value": 37.399,
            "range": "± 0.078",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080628\nmean 37.379 ns"
          },
          {
            "name": "MemoryCacheAccessBenchmarks.Miss",
            "value": 36.526,
            "range": "± 0.135",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080628\nmean 36.515 ns"
          },
          {
            "name": "MemoryCacheAccessBenchmarks.Miss_MicrosoftExtensions",
            "value": 34.223,
            "range": "± 0.153",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080628\nmean 34.182 ns"
          },
          {
            "name": "MemoryCacheAccessBenchmarks.SetSized",
            "value": 135.493,
            "range": "± 0.622",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080628\nmean 135.455 ns"
          },
          {
            "name": "MemoryCacheAccessBenchmarks.SetSized_MicrosoftExtensions",
            "value": 79.927,
            "range": "± 0.339",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080628\nmean 79.848 ns"
          },
          {
            "name": "MemoryCacheAccessBenchmarks.SetWithoutExpiration",
            "value": 131.894,
            "range": "± 0.33",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080628\nmean 131.782 ns"
          },
          {
            "name": "MemoryCacheAccessBenchmarks.SetWithoutExpiration_MicrosoftExtensions",
            "value": 79.411,
            "range": "± 0.185",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080628\nmean 79.405 ns"
          },
          {
            "name": "MemoryCacheAccessBenchmarks.SlidingHit",
            "value": 71.364,
            "range": "± 0.236",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080628\nmean 71.322 ns"
          },
          {
            "name": "MemoryCacheAccessBenchmarks.SlidingHit_MicrosoftExtensions",
            "value": 36.656,
            "range": "± 0.108",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080628\nmean 36.62 ns"
          },
          {
            "name": "MemoryCacheBenchmarks.TryGetValue_MicrosoftExtensions",
            "value": 38.738,
            "range": "± 0.051",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080628\nmean 38.755 ns"
          },
          {
            "name": "MemoryCacheBenchmarks.TryGetValue",
            "value": 79.059,
            "range": "± 0.244",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080628\nmean 79.007 ns"
          },
          {
            "name": "MemoryCacheBenchmarks.Set_MicrosoftExtensions",
            "value": 176.063,
            "range": "± 0.474",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080628\nmean 176.107 ns"
          },
          {
            "name": "MemoryCacheBenchmarks.Set",
            "value": 140.795,
            "range": "± 0.518",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080628\nmean 140.857 ns"
          },
          {
            "name": "MemoryCacheCapacityBenchmarks.InsertWithEviction(EntryCount=32)",
            "value": 11766.5,
            "range": "± 182.233",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080628\nmean 11730.556 ns"
          },
          {
            "name": "MemoryCacheCapacityBenchmarks.InsertWithEviction(EntryCount=1024)",
            "value": 463336,
            "range": "± 15457.871",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080628\nmean 470621.15 ns"
          },
          {
            "name": "MemoryCacheEvictionBenchmarks.Clear(EntryCount=32)",
            "value": 9096.5,
            "range": "± 156.769",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080628\nmean 9135.65 ns"
          },
          {
            "name": "MemoryCacheEvictionBenchmarks.Clear_MicrosoftExtensions(EntryCount=32)",
            "value": 7900.5,
            "range": "± 962.487",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080628\nmean 8469.05 ns"
          },
          {
            "name": "MemoryCacheEvictionBenchmarks.Clear(EntryCount=1024)",
            "value": 168241,
            "range": "± 1955.922",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080628\nmean 168802.167 ns"
          },
          {
            "name": "MemoryCacheEvictionBenchmarks.Clear_MicrosoftExtensions(EntryCount=1024)",
            "value": 117241,
            "range": "± 636.499",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080628\nmean 117300.882 ns"
          },
          {
            "name": "MemoryCacheEvictionBenchmarks.Remove(EntryCount=32)",
            "value": 3947,
            "range": "± 79.954",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080628\nmean 3949.5 ns"
          },
          {
            "name": "MemoryCacheEvictionBenchmarks.Remove_MicrosoftExtensions(EntryCount=32)",
            "value": 6787,
            "range": "± 987.675",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080628\nmean 6214.2 ns"
          },
          {
            "name": "MemoryCacheEvictionBenchmarks.Remove(EntryCount=1024)",
            "value": 104697,
            "range": "± 19786.981",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080628\nmean 93749.684 ns"
          },
          {
            "name": "MemoryCacheEvictionBenchmarks.Remove_MicrosoftExtensions(EntryCount=1024)",
            "value": 151319.5,
            "range": "± 9655.055",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080628\nmean 150994.088 ns"
          },
          {
            "name": "MemoryCacheExpirationBenchmarks.ScanLive(EntryCount=32)",
            "value": 4323.5,
            "range": "± 102.651",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080628\nmean 4329.1 ns"
          },
          {
            "name": "MemoryCacheExpirationBenchmarks.ScanExpired(EntryCount=32)",
            "value": 9943,
            "range": "± 1675.086",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080628\nmean 10366.4 ns"
          },
          {
            "name": "MemoryCacheExpirationBenchmarks.ScanLive(EntryCount=1024)",
            "value": 65933,
            "range": "± 3335.015",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080628\nmean 65737.353 ns"
          },
          {
            "name": "MemoryCacheExpirationBenchmarks.ScanExpired(EntryCount=1024)",
            "value": 149041,
            "range": "± 7411.539",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080628\nmean 151677.824 ns"
          },
          {
            "name": "TypedCacheBenchmarks.GetTyped(Hit=False)",
            "value": 41.719,
            "range": "± 0.133",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080628\nmean 41.683 ns"
          },
          {
            "name": "TypedCacheBenchmarks.GetTyped_MicrosoftExtensions(Hit=False)",
            "value": 35.447,
            "range": "± 0.013",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080628\nmean 35.444 ns"
          },
          {
            "name": "TypedCacheBenchmarks.GetTyped(Hit=True)",
            "value": 72.618,
            "range": "± 0.32",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080628\nmean 72.507 ns"
          },
          {
            "name": "TypedCacheBenchmarks.GetTyped_MicrosoftExtensions(Hit=True)",
            "value": 40.004,
            "range": "± 0.13",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080628\nmean 39.98 ns"
          },
          {
            "name": "TypedCacheBenchmarks.TryGetTyped(Hit=False)",
            "value": 41.712,
            "range": "± 0.174",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080628\nmean 41.678 ns"
          },
          {
            "name": "TypedCacheBenchmarks.TryGetTyped_MicrosoftExtensions(Hit=False)",
            "value": 35.7,
            "range": "± 0.134",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080628\nmean 35.647 ns"
          },
          {
            "name": "TypedCacheBenchmarks.TryGetTyped(Hit=True)",
            "value": 72.351,
            "range": "± 0.428",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080628\nmean 72.292 ns"
          },
          {
            "name": "TypedCacheBenchmarks.TryGetTyped_MicrosoftExtensions(Hit=True)",
            "value": 39.135,
            "range": "± 0.121",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080628\nmean 39.101 ns"
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
          "id": "699980c0cf28b76854e64dc51e4e45d2c5c05dee",
          "message": "Merge pull request #8 from assimalign/feature/benchmark-suite-expansion\n\nBuild out the benchmark suites: 148 to 988 cases across every area but Database",
          "timestamp": "2026-10-20T07:59:35Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/699980c0cf28b76854e64dc51e4e45d2c5c05dee"
        },
        "date": 1792485061280,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "GetOrCreateBenchmarks.Create - allocated",
            "value": 512,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080628"
          },
          {
            "name": "GetOrCreateBenchmarks.Create_MicrosoftExtensions - allocated",
            "value": 152,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080628"
          },
          {
            "name": "GetOrCreateHitBenchmarks.Hit - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080628"
          },
          {
            "name": "GetOrCreateHitBenchmarks.Hit_MicrosoftExtensions - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080628"
          },
          {
            "name": "MemoryCacheAccessBenchmarks.Miss - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080628"
          },
          {
            "name": "MemoryCacheAccessBenchmarks.Miss_MicrosoftExtensions - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080628"
          },
          {
            "name": "MemoryCacheAccessBenchmarks.SetSized - allocated",
            "value": 400,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080628"
          },
          {
            "name": "MemoryCacheAccessBenchmarks.SetSized_MicrosoftExtensions - allocated",
            "value": 104,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080628"
          },
          {
            "name": "MemoryCacheAccessBenchmarks.SetWithoutExpiration - allocated",
            "value": 400,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080628"
          },
          {
            "name": "MemoryCacheAccessBenchmarks.SetWithoutExpiration_MicrosoftExtensions - allocated",
            "value": 104,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080628"
          },
          {
            "name": "MemoryCacheAccessBenchmarks.SlidingHit - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080628"
          },
          {
            "name": "MemoryCacheAccessBenchmarks.SlidingHit_MicrosoftExtensions - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080628"
          },
          {
            "name": "MemoryCacheBenchmarks.TryGetValue_MicrosoftExtensions - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080628"
          },
          {
            "name": "MemoryCacheBenchmarks.TryGetValue - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080628"
          },
          {
            "name": "MemoryCacheBenchmarks.Set_MicrosoftExtensions - allocated",
            "value": 104,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080628"
          },
          {
            "name": "MemoryCacheBenchmarks.Set - allocated",
            "value": 400,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080628"
          },
          {
            "name": "MemoryCacheCapacityBenchmarks.InsertWithEviction(EntryCount=32) - allocated",
            "value": 5776,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080628"
          },
          {
            "name": "MemoryCacheCapacityBenchmarks.InsertWithEviction(EntryCount=1024) - allocated",
            "value": 14096,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080628"
          },
          {
            "name": "MemoryCacheEvictionBenchmarks.Clear(EntryCount=32) - allocated",
            "value": 5392,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080628"
          },
          {
            "name": "MemoryCacheEvictionBenchmarks.Clear_MicrosoftExtensions(EntryCount=32) - allocated",
            "value": 1400,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080628"
          },
          {
            "name": "MemoryCacheEvictionBenchmarks.Clear(EntryCount=1024) - allocated",
            "value": 5392,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080628"
          },
          {
            "name": "MemoryCacheEvictionBenchmarks.Clear_MicrosoftExtensions(EntryCount=1024) - allocated",
            "value": 6728,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080628"
          },
          {
            "name": "MemoryCacheEvictionBenchmarks.Remove(EntryCount=32) - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080628"
          },
          {
            "name": "MemoryCacheEvictionBenchmarks.Remove_MicrosoftExtensions(EntryCount=32) - allocated",
            "value": 672,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080628"
          },
          {
            "name": "MemoryCacheEvictionBenchmarks.Remove(EntryCount=1024) - allocated",
            "value": 5328,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080628"
          },
          {
            "name": "MemoryCacheEvictionBenchmarks.Remove_MicrosoftExtensions(EntryCount=1024) - allocated",
            "value": 5328,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080628"
          },
          {
            "name": "MemoryCacheExpirationBenchmarks.ScanLive(EntryCount=32) - allocated",
            "value": 64,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080628"
          },
          {
            "name": "MemoryCacheExpirationBenchmarks.ScanExpired(EntryCount=32) - allocated",
            "value": 688,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080628"
          },
          {
            "name": "MemoryCacheExpirationBenchmarks.ScanLive(EntryCount=1024) - allocated",
            "value": 5728,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080628"
          },
          {
            "name": "MemoryCacheExpirationBenchmarks.ScanExpired(EntryCount=1024) - allocated",
            "value": 5728,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080628"
          },
          {
            "name": "TypedCacheBenchmarks.GetTyped(Hit=False) - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080628"
          },
          {
            "name": "TypedCacheBenchmarks.GetTyped_MicrosoftExtensions(Hit=False) - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080628"
          },
          {
            "name": "TypedCacheBenchmarks.GetTyped(Hit=True) - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080628"
          },
          {
            "name": "TypedCacheBenchmarks.GetTyped_MicrosoftExtensions(Hit=True) - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080628"
          },
          {
            "name": "TypedCacheBenchmarks.TryGetTyped(Hit=False) - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080628"
          },
          {
            "name": "TypedCacheBenchmarks.TryGetTyped_MicrosoftExtensions(Hit=False) - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080628"
          },
          {
            "name": "TypedCacheBenchmarks.TryGetTyped(Hit=True) - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080628"
          },
          {
            "name": "TypedCacheBenchmarks.TryGetTyped_MicrosoftExtensions(Hit=True) - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080628"
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
          "id": "699980c0cf28b76854e64dc51e4e45d2c5c05dee",
          "message": "Merge pull request #8 from assimalign/feature/benchmark-suite-expansion\n\nBuild out the benchmark suites: 148 to 988 cases across every area but Database",
          "timestamp": "2026-10-20T07:59:35Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/699980c0cf28b76854e64dc51e4e45d2c5c05dee"
        },
        "date": 1792485074280,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "CommandLineConfigurationBenchmarks.Build(KeyCount=8)",
            "value": 5989.075,
            "range": "± 71.004",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080631\nmean 5971.513 ns"
          },
          {
            "name": "CommandLineConfigurationBenchmarks.Build_MicrosoftExtensions(KeyCount=8)",
            "value": 1057.262,
            "range": "± 17.286",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080631\nmean 1053.782 ns"
          },
          {
            "name": "CommandLineConfigurationBenchmarks.Build(KeyCount=128)",
            "value": 65583.86,
            "range": "± 865.721",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080631\nmean 65990.881 ns"
          },
          {
            "name": "CommandLineConfigurationBenchmarks.Build_MicrosoftExtensions(KeyCount=128)",
            "value": 10972.621,
            "range": "± 236.483",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080631\nmean 10966.083 ns"
          },
          {
            "name": "ConfigurationBindingBenchmarks.Bind_MicrosoftExtensions",
            "value": 14117.305,
            "range": "± 146.981",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080631\nmean 14107.12 ns"
          },
          {
            "name": "ConfigurationBindingBenchmarks.Bind",
            "value": 21718.156,
            "range": "± 246.423",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080631\nmean 21791.012 ns"
          },
          {
            "name": "ConfigurationLookupBenchmarks.GetValue_MicrosoftExtensions",
            "value": 32.53,
            "range": "± 0.029",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080631\nmean 32.533 ns"
          },
          {
            "name": "ConfigurationLookupBenchmarks.GetValue",
            "value": 468.949,
            "range": "± 10.269",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080631\nmean 467.904 ns"
          },
          {
            "name": "EnvironmentConfigurationBenchmarks.Build(KeyCount=8)",
            "value": 25471.423,
            "range": "± 280.851",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080631\nmean 25575.925 ns"
          },
          {
            "name": "EnvironmentConfigurationBenchmarks.Build_MicrosoftExtensions(KeyCount=8)",
            "value": 20856.064,
            "range": "± 229.961",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080631\nmean 20839.639 ns"
          },
          {
            "name": "EnvironmentConfigurationBenchmarks.Build(KeyCount=128)",
            "value": 121485.149,
            "range": "± 911.948",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080631\nmean 121527.736 ns"
          },
          {
            "name": "EnvironmentConfigurationBenchmarks.Build_MicrosoftExtensions(KeyCount=128)",
            "value": 59642.443,
            "range": "± 325.421",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080631\nmean 59572.426 ns"
          },
          {
            "name": "FileConfigurationBenchmarks.Build(KeyCount=8)",
            "value": 91135.253,
            "range": "± 3492.929",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080631\nmean 90875.541 ns"
          },
          {
            "name": "FileConfigurationBenchmarks.Build_MicrosoftExtensions(KeyCount=8)",
            "value": 19676.574,
            "range": "± 30.821",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080631\nmean 19676.144 ns"
          },
          {
            "name": "FileConfigurationBenchmarks.Build(KeyCount=128)",
            "value": 152742.765,
            "range": "± 5743.989",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080631\nmean 154144.379 ns"
          },
          {
            "name": "FileConfigurationBenchmarks.Build_MicrosoftExtensions(KeyCount=128)",
            "value": 45908.735,
            "range": "± 427.554",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080631\nmean 45836.59 ns"
          },
          {
            "name": "FileConfigurationBenchmarks.Reload(KeyCount=8)",
            "value": 66689.162,
            "range": "± 2570.528",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080631\nmean 66260.108 ns"
          },
          {
            "name": "FileConfigurationBenchmarks.Reload_MicrosoftExtensions(KeyCount=8)",
            "value": 13765.453,
            "range": "± 55.607",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080631\nmean 13773.613 ns"
          },
          {
            "name": "FileConfigurationBenchmarks.Reload(KeyCount=128)",
            "value": 136220.924,
            "range": "± 9077.674",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080631\nmean 137123.924 ns"
          },
          {
            "name": "FileConfigurationBenchmarks.Reload_MicrosoftExtensions(KeyCount=128)",
            "value": 38731.368,
            "range": "± 525.228",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080631\nmean 38931.228 ns"
          },
          {
            "name": "IniConfigurationBenchmarks.Build(KeyCount=8)",
            "value": 6192.709,
            "range": "± 67.756",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080631\nmean 6202.011 ns"
          },
          {
            "name": "IniConfigurationBenchmarks.Build_MicrosoftExtensions(KeyCount=8)",
            "value": 1899.407,
            "range": "± 32.909",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080631\nmean 1891.315 ns"
          },
          {
            "name": "IniConfigurationBenchmarks.Build(KeyCount=128)",
            "value": 54356.164,
            "range": "± 700.303",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080631\nmean 54544.03 ns"
          },
          {
            "name": "IniConfigurationBenchmarks.Build_MicrosoftExtensions(KeyCount=128)",
            "value": 16405.305,
            "range": "± 226.661",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080631\nmean 16432.652 ns"
          },
          {
            "name": "JsonConfigurationBenchmarks.Build_MicrosoftExtensions",
            "value": 6559.263,
            "range": "± 79.219",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080631\nmean 6547.475 ns"
          },
          {
            "name": "JsonConfigurationBenchmarks.Build",
            "value": 16035.218,
            "range": "± 244.91",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080631\nmean 16062.146 ns"
          },
          {
            "name": "PathBenchmarks.Parse(value=/Serv(...)\\0:Url [23])",
            "value": 296.46,
            "range": "± 5.717",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080631\nmean 297.539 ns"
          },
          {
            "name": "PathBenchmarks.Parse(value=Servi(...)quest [24])",
            "value": 299.855,
            "range": "± 8.701",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080631\nmean 302.843 ns"
          },
          {
            "name": "SectionEnumerationBenchmarks.Enumerate(KeyCount=8)",
            "value": 154.106,
            "range": "± 2.227",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080631\nmean 154.651 ns"
          },
          {
            "name": "SectionEnumerationBenchmarks.Enumerate_MicrosoftExtensions(KeyCount=8)",
            "value": 1229.148,
            "range": "± 9.19",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080631\nmean 1229.149 ns"
          },
          {
            "name": "SectionEnumerationBenchmarks.Enumerate(KeyCount=128)",
            "value": 271.922,
            "range": "± 4.372",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080631\nmean 273.843 ns"
          },
          {
            "name": "SectionEnumerationBenchmarks.Enumerate_MicrosoftExtensions(KeyCount=128)",
            "value": 34277.177,
            "range": "± 75.26",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080631\nmean 34291.576 ns"
          },
          {
            "name": "SectionEnumerationBenchmarks.GetMissingValue(KeyCount=8)",
            "value": 313.902,
            "range": "± 4.14",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080631\nmean 313.291 ns"
          },
          {
            "name": "SectionEnumerationBenchmarks.GetMissingValue_MicrosoftExtensions(KeyCount=8)",
            "value": 20.778,
            "range": "± 0.008",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080631\nmean 20.776 ns"
          },
          {
            "name": "SectionEnumerationBenchmarks.GetMissingValue(KeyCount=128)",
            "value": 303.815,
            "range": "± 3.506",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080631\nmean 304.408 ns"
          },
          {
            "name": "SectionEnumerationBenchmarks.GetMissingValue_MicrosoftExtensions(KeyCount=128)",
            "value": 24.98,
            "range": "± 0.059",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080631\nmean 25.006 ns"
          },
          {
            "name": "XmlConfigurationBenchmarks.Build(KeyCount=8)",
            "value": 13321.232,
            "range": "± 108.109",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080631\nmean 13347.389 ns"
          },
          {
            "name": "XmlConfigurationBenchmarks.Build_MicrosoftExtensions(KeyCount=8)",
            "value": 8838.623,
            "range": "± 37.641",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080631\nmean 8844.568 ns"
          },
          {
            "name": "XmlConfigurationBenchmarks.Build(KeyCount=128)",
            "value": 125694.357,
            "range": "± 1095.547",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080631\nmean 125945.028 ns"
          },
          {
            "name": "XmlConfigurationBenchmarks.Build_MicrosoftExtensions(KeyCount=128)",
            "value": 85276.167,
            "range": "± 800.019",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080631\nmean 85144.983 ns"
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
          "id": "699980c0cf28b76854e64dc51e4e45d2c5c05dee",
          "message": "Merge pull request #8 from assimalign/feature/benchmark-suite-expansion\n\nBuild out the benchmark suites: 148 to 988 cases across every area but Database",
          "timestamp": "2026-10-20T07:59:35Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/699980c0cf28b76854e64dc51e4e45d2c5c05dee"
        },
        "date": 1792485076406,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "CommandLineConfigurationBenchmarks.Build(KeyCount=8) - allocated",
            "value": 14776,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080631"
          },
          {
            "name": "CommandLineConfigurationBenchmarks.Build_MicrosoftExtensions(KeyCount=8) - allocated",
            "value": 3280,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080631"
          },
          {
            "name": "CommandLineConfigurationBenchmarks.Build(KeyCount=128) - allocated",
            "value": 167558,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080631"
          },
          {
            "name": "CommandLineConfigurationBenchmarks.Build_MicrosoftExtensions(KeyCount=128) - allocated",
            "value": 30704,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080631"
          },
          {
            "name": "ConfigurationBindingBenchmarks.Bind_MicrosoftExtensions - allocated",
            "value": 11490,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080631"
          },
          {
            "name": "ConfigurationBindingBenchmarks.Bind - allocated",
            "value": 18212,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080631"
          },
          {
            "name": "ConfigurationLookupBenchmarks.GetValue_MicrosoftExtensions - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080631"
          },
          {
            "name": "ConfigurationLookupBenchmarks.GetValue - allocated",
            "value": 1088,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080631"
          },
          {
            "name": "EnvironmentConfigurationBenchmarks.Build(KeyCount=8) - allocated",
            "value": 45809,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080631"
          },
          {
            "name": "EnvironmentConfigurationBenchmarks.Build_MicrosoftExtensions(KeyCount=8) - allocated",
            "value": 30688,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080631"
          },
          {
            "name": "EnvironmentConfigurationBenchmarks.Build(KeyCount=128) - allocated",
            "value": 230080,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080631"
          },
          {
            "name": "EnvironmentConfigurationBenchmarks.Build_MicrosoftExtensions(KeyCount=128) - allocated",
            "value": 98224,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080631"
          },
          {
            "name": "FileConfigurationBenchmarks.Build(KeyCount=8) - allocated",
            "value": 47069,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080631"
          },
          {
            "name": "FileConfigurationBenchmarks.Build_MicrosoftExtensions(KeyCount=8) - allocated",
            "value": 8560,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080631"
          },
          {
            "name": "FileConfigurationBenchmarks.Build(KeyCount=128) - allocated",
            "value": 147058,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080631"
          },
          {
            "name": "FileConfigurationBenchmarks.Build_MicrosoftExtensions(KeyCount=128) - allocated",
            "value": 46746,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080631"
          },
          {
            "name": "FileConfigurationBenchmarks.Reload(KeyCount=8) - allocated",
            "value": 25709,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080631"
          },
          {
            "name": "FileConfigurationBenchmarks.Reload_MicrosoftExtensions(KeyCount=8) - allocated",
            "value": 7248,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080631"
          },
          {
            "name": "FileConfigurationBenchmarks.Reload(KeyCount=128) - allocated",
            "value": 125709,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080631"
          },
          {
            "name": "FileConfigurationBenchmarks.Reload_MicrosoftExtensions(KeyCount=128) - allocated",
            "value": 45433,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080631"
          },
          {
            "name": "IniConfigurationBenchmarks.Build(KeyCount=8) - allocated",
            "value": 16407,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080631"
          },
          {
            "name": "IniConfigurationBenchmarks.Build_MicrosoftExtensions(KeyCount=8) - allocated",
            "value": 6944,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080631"
          },
          {
            "name": "IniConfigurationBenchmarks.Build(KeyCount=128) - allocated",
            "value": 131731,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080631"
          },
          {
            "name": "IniConfigurationBenchmarks.Build_MicrosoftExtensions(KeyCount=128) - allocated",
            "value": 39137,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080631"
          },
          {
            "name": "JsonConfigurationBenchmarks.Build_MicrosoftExtensions - allocated",
            "value": 12536,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080631"
          },
          {
            "name": "JsonConfigurationBenchmarks.Build - allocated",
            "value": 26873,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080631"
          },
          {
            "name": "PathBenchmarks.Parse(value=/Serv(...)\\0:Url [23]) - allocated",
            "value": 1000,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080631"
          },
          {
            "name": "PathBenchmarks.Parse(value=Servi(...)quest [24]) - allocated",
            "value": 1000,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080631"
          },
          {
            "name": "SectionEnumerationBenchmarks.Enumerate(KeyCount=8) - allocated",
            "value": 392,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080631"
          },
          {
            "name": "SectionEnumerationBenchmarks.Enumerate_MicrosoftExtensions(KeyCount=8) - allocated",
            "value": 2136,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080631"
          },
          {
            "name": "SectionEnumerationBenchmarks.Enumerate(KeyCount=128) - allocated",
            "value": 392,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080631"
          },
          {
            "name": "SectionEnumerationBenchmarks.Enumerate_MicrosoftExtensions(KeyCount=128) - allocated",
            "value": 27056,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080631"
          },
          {
            "name": "SectionEnumerationBenchmarks.GetMissingValue(KeyCount=8) - allocated",
            "value": 768,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080631"
          },
          {
            "name": "SectionEnumerationBenchmarks.GetMissingValue_MicrosoftExtensions(KeyCount=8) - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080631"
          },
          {
            "name": "SectionEnumerationBenchmarks.GetMissingValue(KeyCount=128) - allocated",
            "value": 768,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080631"
          },
          {
            "name": "SectionEnumerationBenchmarks.GetMissingValue_MicrosoftExtensions(KeyCount=128) - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080631"
          },
          {
            "name": "XmlConfigurationBenchmarks.Build(KeyCount=8) - allocated",
            "value": 28032,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080631"
          },
          {
            "name": "XmlConfigurationBenchmarks.Build_MicrosoftExtensions(KeyCount=8) - allocated",
            "value": 19640,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080631"
          },
          {
            "name": "XmlConfigurationBenchmarks.Build(KeyCount=128) - allocated",
            "value": 240103,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080631"
          },
          {
            "name": "XmlConfigurationBenchmarks.Build_MicrosoftExtensions(KeyCount=128) - allocated",
            "value": 157219,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080631"
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
          "id": "699980c0cf28b76854e64dc51e4e45d2c5c05dee",
          "message": "Merge pull request #8 from assimalign/feature/benchmark-suite-expansion\n\nBuild out the benchmark suites: 148 to 988 cases across every area but Database",
          "timestamp": "2026-10-20T07:59:35Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/699980c0cf28b76854e64dc51e4e45d2c5c05dee"
        },
        "date": 1792485096252,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "ConnectionFeatureLookupBenchmarks.NamedHit(Layers=1)",
            "value": 8.622,
            "range": "± 0.017",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080635\nmean 8.632 ns"
          },
          {
            "name": "ConnectionFeatureLookupBenchmarks.TypedHit(Layers=1)",
            "value": 19.55,
            "range": "± 0.036",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080635\nmean 19.558 ns"
          },
          {
            "name": "ConnectionFeatureLookupBenchmarks.NamedMiss(Layers=1)",
            "value": 6.21,
            "range": "± 0.004",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080635\nmean 6.21 ns"
          },
          {
            "name": "ConnectionFeatureLookupBenchmarks.TypedMiss(Layers=1)",
            "value": 30.88,
            "range": "± 0.054",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080635\nmean 30.891 ns"
          },
          {
            "name": "ConnectionFeatureLookupBenchmarks.NamedHit(Layers=4)",
            "value": 18.906,
            "range": "± 0.142",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080635\nmean 18.88 ns"
          },
          {
            "name": "ConnectionFeatureLookupBenchmarks.TypedHit(Layers=4)",
            "value": 61.753,
            "range": "± 0.06",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080635\nmean 61.779 ns"
          },
          {
            "name": "ConnectionFeatureLookupBenchmarks.NamedMiss(Layers=4)",
            "value": 6.223,
            "range": "± 0.013",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080635\nmean 6.228 ns"
          },
          {
            "name": "ConnectionFeatureLookupBenchmarks.TypedMiss(Layers=4)",
            "value": 67.223,
            "range": "± 0.161",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080635\nmean 67.26 ns"
          },
          {
            "name": "DuplexPipeStreamBenchmarks.Read(PayloadSize=64)",
            "value": 228.035,
            "range": "± 0.455",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080635\nmean 228.106 ns"
          },
          {
            "name": "DuplexPipeStreamBenchmarks.Read_SystemIOPipelines(PayloadSize=64)",
            "value": 237.021,
            "range": "± 0.593",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080635\nmean 237.215 ns"
          },
          {
            "name": "DuplexPipeStreamBenchmarks.Read(PayloadSize=16384)",
            "value": 1395.305,
            "range": "± 3.187",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080635\nmean 1396.533 ns"
          },
          {
            "name": "DuplexPipeStreamBenchmarks.Read_SystemIOPipelines(PayloadSize=16384)",
            "value": 1377.61,
            "range": "± 2.203",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080635\nmean 1378.257 ns"
          },
          {
            "name": "DuplexPipeStreamBenchmarks.Write(PayloadSize=64)",
            "value": 213.358,
            "range": "± 0.596",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080635\nmean 213.559 ns"
          },
          {
            "name": "DuplexPipeStreamBenchmarks.Write_SystemIOPipelines(PayloadSize=64)",
            "value": 187.477,
            "range": "± 0.235",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080635\nmean 187.454 ns"
          },
          {
            "name": "DuplexPipeStreamBenchmarks.Write(PayloadSize=16384)",
            "value": 1270.351,
            "range": "± 2.333",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080635\nmean 1270.426 ns"
          },
          {
            "name": "DuplexPipeStreamBenchmarks.Write_SystemIOPipelines(PayloadSize=16384)",
            "value": 1240.488,
            "range": "± 2.155",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080635\nmean 1241.202 ns"
          },
          {
            "name": "InMemoryBufferBenchmarks.Transfer(PayloadSize=64)",
            "value": 194.254,
            "range": "± 0.369",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080635\nmean 194.273 ns"
          },
          {
            "name": "InMemoryBufferBenchmarks.Transfer_SystemIOPipelines(PayloadSize=64)",
            "value": 211.476,
            "range": "± 0.398",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080635\nmean 211.602 ns"
          },
          {
            "name": "InMemoryBufferBenchmarks.Transfer(PayloadSize=16384)",
            "value": 1436.772,
            "range": "± 1.297",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080635\nmean 1437.024 ns"
          },
          {
            "name": "InMemoryBufferBenchmarks.Transfer_SystemIOPipelines(PayloadSize=16384)",
            "value": 1382.731,
            "range": "± 2.026",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080635\nmean 1383.273 ns"
          },
          {
            "name": "InMemoryBufferBenchmarks.TryReadTransfer(PayloadSize=64)",
            "value": 167.865,
            "range": "± 0.675",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080635\nmean 167.942 ns"
          },
          {
            "name": "InMemoryBufferBenchmarks.TryReadTransfer_SystemIOPipelines(PayloadSize=64)",
            "value": 189.367,
            "range": "± 0.345",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080635\nmean 189.492 ns"
          },
          {
            "name": "InMemoryBufferBenchmarks.TryReadTransfer(PayloadSize=16384)",
            "value": 1394.558,
            "range": "± 2.148",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080635\nmean 1395.459 ns"
          },
          {
            "name": "InMemoryBufferBenchmarks.TryReadTransfer_SystemIOPipelines(PayloadSize=16384)",
            "value": 1372.045,
            "range": "± 3.737",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080635\nmean 1373.95 ns"
          },
          {
            "name": "InMemoryConnectionBenchmarks.RoundTrip_SystemIOPipelines(PayloadSize=64)",
            "value": 392.571,
            "range": "± 1.286",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080635\nmean 392.342 ns"
          },
          {
            "name": "InMemoryConnectionBenchmarks.RoundTrip(PayloadSize=64)",
            "value": 413.04,
            "range": "± 0.909",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080635\nmean 413.284 ns"
          },
          {
            "name": "InMemoryConnectionBenchmarks.RoundTrip_SystemIOPipelines(PayloadSize=4096)",
            "value": 519.418,
            "range": "± 1.889",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080635\nmean 518.821 ns"
          },
          {
            "name": "InMemoryConnectionBenchmarks.RoundTrip(PayloadSize=4096)",
            "value": 560.442,
            "range": "± 0.974",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080635\nmean 560.46 ns"
          },
          {
            "name": "InMemoryConnectionListenerBenchmarks.ConnectAccept",
            "value": 844.854,
            "range": "± 11.972",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080635\nmean 844.709 ns"
          },
          {
            "name": "InMemoryMultiplexedStreamBenchmarks.RoundTrip(PayloadSize=64)",
            "value": 443.414,
            "range": "± 0.603",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080635\nmean 443.626 ns"
          },
          {
            "name": "InMemoryMultiplexedStreamBenchmarks.RoundTrip_SystemIOPipelines(PayloadSize=64)",
            "value": 464.2,
            "range": "± 3.866",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080635\nmean 465.994 ns"
          },
          {
            "name": "InMemoryMultiplexedStreamBenchmarks.RoundTrip(PayloadSize=4096)",
            "value": 1394.028,
            "range": "± 0.463",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080635\nmean 1394.041 ns"
          },
          {
            "name": "InMemoryMultiplexedStreamBenchmarks.RoundTrip_SystemIOPipelines(PayloadSize=4096)",
            "value": 719.936,
            "range": "± 0.574",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080635\nmean 720.022 ns"
          },
          {
            "name": "InMemoryStreamLifecycleBenchmarks.OpenAccept(Direction=Bidirectional)",
            "value": 820.959,
            "range": "± 9.125",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080635\nmean 818.812 ns"
          },
          {
            "name": "InMemoryStreamLifecycleBenchmarks.OpenAccept(Direction=WriteOnly)",
            "value": 835.696,
            "range": "± 6.894",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080635\nmean 834.742 ns"
          },
          {
            "name": "StreamAbortReadBenchmarks.ReadAfterLocalAbort",
            "value": 2976.378,
            "range": "± 7.592",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080635\nmean 2977.908 ns"
          },
          {
            "name": "StreamAbortReadBenchmarks.ReadAfterPeerReset",
            "value": 2599.805,
            "range": "± 3.087",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080635\nmean 2599.318 ns"
          },
          {
            "name": "StreamLimitLookupBenchmarks.CheckStreamOrdinals(FeaturePresent=False)",
            "value": 5.03,
            "range": "± 0.022",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080635\nmean 5.043 ns"
          },
          {
            "name": "StreamLimitLookupBenchmarks.CheckStreamOrdinals(FeaturePresent=True)",
            "value": 10.709,
            "range": "± 0.061",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080635\nmean 10.741 ns"
          },
          {
            "name": "TlsFeatureLookupBenchmarks.SelectHttp2(FeaturePresent=False)",
            "value": 4.091,
            "range": "± 0.002",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080635\nmean 4.092 ns"
          },
          {
            "name": "TlsFeatureLookupBenchmarks.SelectHttp2(FeaturePresent=True)",
            "value": 14.817,
            "range": "± 0.098",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080635\nmean 14.798 ns"
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
          "id": "699980c0cf28b76854e64dc51e4e45d2c5c05dee",
          "message": "Merge pull request #8 from assimalign/feature/benchmark-suite-expansion\n\nBuild out the benchmark suites: 148 to 988 cases across every area but Database",
          "timestamp": "2026-10-20T07:59:35Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/699980c0cf28b76854e64dc51e4e45d2c5c05dee"
        },
        "date": 1792485100598,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "ConnectionFeatureLookupBenchmarks.NamedHit(Layers=1) - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080635"
          },
          {
            "name": "ConnectionFeatureLookupBenchmarks.TypedHit(Layers=1) - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080635"
          },
          {
            "name": "ConnectionFeatureLookupBenchmarks.NamedMiss(Layers=1) - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080635"
          },
          {
            "name": "ConnectionFeatureLookupBenchmarks.TypedMiss(Layers=1) - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080635"
          },
          {
            "name": "ConnectionFeatureLookupBenchmarks.NamedHit(Layers=4) - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080635"
          },
          {
            "name": "ConnectionFeatureLookupBenchmarks.TypedHit(Layers=4) - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080635"
          },
          {
            "name": "ConnectionFeatureLookupBenchmarks.NamedMiss(Layers=4) - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080635"
          },
          {
            "name": "ConnectionFeatureLookupBenchmarks.TypedMiss(Layers=4) - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080635"
          },
          {
            "name": "DuplexPipeStreamBenchmarks.Read(PayloadSize=64) - allocated",
            "value": 72,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080635"
          },
          {
            "name": "DuplexPipeStreamBenchmarks.Read_SystemIOPipelines(PayloadSize=64) - allocated",
            "value": 72,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080635"
          },
          {
            "name": "DuplexPipeStreamBenchmarks.Read(PayloadSize=16384) - allocated",
            "value": 72,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080635"
          },
          {
            "name": "DuplexPipeStreamBenchmarks.Read_SystemIOPipelines(PayloadSize=16384) - allocated",
            "value": 72,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080635"
          },
          {
            "name": "DuplexPipeStreamBenchmarks.Write(PayloadSize=64) - allocated",
            "value": 72,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080635"
          },
          {
            "name": "DuplexPipeStreamBenchmarks.Write_SystemIOPipelines(PayloadSize=64) - allocated",
            "value": 72,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080635"
          },
          {
            "name": "DuplexPipeStreamBenchmarks.Write(PayloadSize=16384) - allocated",
            "value": 72,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080635"
          },
          {
            "name": "DuplexPipeStreamBenchmarks.Write_SystemIOPipelines(PayloadSize=16384) - allocated",
            "value": 72,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080635"
          },
          {
            "name": "InMemoryBufferBenchmarks.Transfer(PayloadSize=64) - allocated",
            "value": 72,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080635"
          },
          {
            "name": "InMemoryBufferBenchmarks.Transfer_SystemIOPipelines(PayloadSize=64) - allocated",
            "value": 72,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080635"
          },
          {
            "name": "InMemoryBufferBenchmarks.Transfer(PayloadSize=16384) - allocated",
            "value": 72,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080635"
          },
          {
            "name": "InMemoryBufferBenchmarks.Transfer_SystemIOPipelines(PayloadSize=16384) - allocated",
            "value": 72,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080635"
          },
          {
            "name": "InMemoryBufferBenchmarks.TryReadTransfer(PayloadSize=64) - allocated",
            "value": 72,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080635"
          },
          {
            "name": "InMemoryBufferBenchmarks.TryReadTransfer_SystemIOPipelines(PayloadSize=64) - allocated",
            "value": 72,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080635"
          },
          {
            "name": "InMemoryBufferBenchmarks.TryReadTransfer(PayloadSize=16384) - allocated",
            "value": 72,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080635"
          },
          {
            "name": "InMemoryBufferBenchmarks.TryReadTransfer_SystemIOPipelines(PayloadSize=16384) - allocated",
            "value": 72,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080635"
          },
          {
            "name": "InMemoryConnectionBenchmarks.RoundTrip_SystemIOPipelines(PayloadSize=64) - allocated",
            "value": 144,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080635"
          },
          {
            "name": "InMemoryConnectionBenchmarks.RoundTrip(PayloadSize=64) - allocated",
            "value": 144,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080635"
          },
          {
            "name": "InMemoryConnectionBenchmarks.RoundTrip_SystemIOPipelines(PayloadSize=4096) - allocated",
            "value": 144,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080635"
          },
          {
            "name": "InMemoryConnectionBenchmarks.RoundTrip(PayloadSize=4096) - allocated",
            "value": 144,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080635"
          },
          {
            "name": "InMemoryConnectionListenerBenchmarks.ConnectAccept - allocated",
            "value": 1848,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080635"
          },
          {
            "name": "InMemoryMultiplexedStreamBenchmarks.RoundTrip(PayloadSize=64) - allocated",
            "value": 72,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080635"
          },
          {
            "name": "InMemoryMultiplexedStreamBenchmarks.RoundTrip_SystemIOPipelines(PayloadSize=64) - allocated",
            "value": 72,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080635"
          },
          {
            "name": "InMemoryMultiplexedStreamBenchmarks.RoundTrip(PayloadSize=4096) - allocated",
            "value": 72,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080635"
          },
          {
            "name": "InMemoryMultiplexedStreamBenchmarks.RoundTrip_SystemIOPipelines(PayloadSize=4096) - allocated",
            "value": 72,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080635"
          },
          {
            "name": "InMemoryStreamLifecycleBenchmarks.OpenAccept(Direction=Bidirectional) - allocated",
            "value": 1696,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080635"
          },
          {
            "name": "InMemoryStreamLifecycleBenchmarks.OpenAccept(Direction=WriteOnly) - allocated",
            "value": 1728,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080635"
          },
          {
            "name": "StreamAbortReadBenchmarks.ReadAfterLocalAbort - allocated",
            "value": 832,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080635"
          },
          {
            "name": "StreamAbortReadBenchmarks.ReadAfterPeerReset - allocated",
            "value": 272,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080635"
          },
          {
            "name": "StreamLimitLookupBenchmarks.CheckStreamOrdinals(FeaturePresent=False) - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080635"
          },
          {
            "name": "StreamLimitLookupBenchmarks.CheckStreamOrdinals(FeaturePresent=True) - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080635"
          },
          {
            "name": "TlsFeatureLookupBenchmarks.SelectHttp2(FeaturePresent=False) - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080635"
          },
          {
            "name": "TlsFeatureLookupBenchmarks.SelectHttp2(FeaturePresent=True) - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080635"
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
          "id": "699980c0cf28b76854e64dc51e4e45d2c5c05dee",
          "message": "Merge pull request #8 from assimalign/feature/benchmark-suite-expansion\n\nBuild out the benchmark suites: 148 to 988 cases across every area but Database",
          "timestamp": "2026-10-20T07:59:35Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/699980c0cf28b76854e64dc51e4e45d2c5c05dee"
        },
        "date": 1792485115362,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "BinaryReaderBenchmarks.ReadRecords(Records=8)",
            "value": 32.067,
            "range": "± 0.027",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080636\nmean 32.071 ns"
          },
          {
            "name": "BinaryReaderBenchmarks.WriteRecords(Records=8)",
            "value": 43.875,
            "range": "± 0.061",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080636\nmean 43.905 ns"
          },
          {
            "name": "BinaryReaderBenchmarks.ReadRecords(Records=128)",
            "value": 327.152,
            "range": "± 0.445",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080636\nmean 327.281 ns"
          },
          {
            "name": "BinaryReaderBenchmarks.WriteRecords(Records=128)",
            "value": 648.76,
            "range": "± 0.853",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080636\nmean 648.956 ns"
          },
          {
            "name": "BmffCursorBenchmarks.WalkHeaders",
            "value": 6219.338,
            "range": "± 17.658",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080636\nmean 6216.188 ns"
          },
          {
            "name": "BmffDocumentBenchmarks.Load",
            "value": 15263.727,
            "range": "± 77.199",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080636\nmean 15241.121 ns"
          },
          {
            "name": "CompositeContentBenchmarks.ReadParts",
            "value": 1278.727,
            "range": "± 7.677",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080636\nmean 1277.924 ns"
          },
          {
            "name": "EbmlReaderBenchmarks.ReadValues",
            "value": 8351.892,
            "range": "± 31.866",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080636\nmean 8357.92 ns"
          },
          {
            "name": "EbmlReaderBenchmarks.Load",
            "value": 10732.444,
            "range": "± 43.316",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080636\nmean 10729.946 ns"
          },
          {
            "name": "ExecutableReaderBenchmarks.Read(Format=ELF)",
            "value": 478.373,
            "range": "± 2.403",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080636\nmean 478.591 ns"
          },
          {
            "name": "ExecutableReaderBenchmarks.Read(Format=MachO)",
            "value": 404.366,
            "range": "± 2.081",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080636\nmean 404.435 ns"
          },
          {
            "name": "ExecutableReaderBenchmarks.Read(Format=PE)",
            "value": 801.153,
            "range": "± 2.016",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080636\nmean 801.441 ns"
          },
          {
            "name": "HeifDocumentBenchmarks.Load(Brand=avif)",
            "value": 18467.953,
            "range": "± 637.17",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080636\nmean 18757.768 ns"
          },
          {
            "name": "HeifDocumentBenchmarks.Load(Brand=heic)",
            "value": 18504.41,
            "range": "± 93.675",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080636\nmean 18513.343 ns"
          },
          {
            "name": "MarkdownBenchmarks.Parse_Markdig",
            "value": 20695.198,
            "range": "± 123.814",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080636\nmean 20716.017 ns"
          },
          {
            "name": "MarkdownBenchmarks.Parse",
            "value": 63981.621,
            "range": "± 188.17",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080636\nmean 63949.554 ns"
          },
          {
            "name": "MarkdownBenchmarks.ToHtml_Markdig",
            "value": 24928.741,
            "range": "± 72.998",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080636\nmean 24935.253 ns"
          },
          {
            "name": "MarkdownBenchmarks.ToHtml",
            "value": 72885.669,
            "range": "± 303.742",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080636\nmean 72794.687 ns"
          },
          {
            "name": "MarkdownShapeBenchmarks.Parse(Shape=Blocks)",
            "value": 110345.858,
            "range": "± 573.292",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080636\nmean 110409.256 ns"
          },
          {
            "name": "MarkdownShapeBenchmarks.Parse_Markdig(Shape=Blocks)",
            "value": 34110.363,
            "range": "± 170.72",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080636\nmean 34052.64 ns"
          },
          {
            "name": "MarkdownShapeBenchmarks.Parse(Shape=Inline)",
            "value": 78671.139,
            "range": "± 283.235",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080636\nmean 78634.341 ns"
          },
          {
            "name": "MarkdownShapeBenchmarks.Parse_Markdig(Shape=Inline)",
            "value": 24534.992,
            "range": "± 118.714",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080636\nmean 24554.418 ns"
          },
          {
            "name": "MarkdownShapeBenchmarks.Render(Shape=Blocks)",
            "value": 8662.357,
            "range": "± 28.345",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080636\nmean 8662.608 ns"
          },
          {
            "name": "MarkdownShapeBenchmarks.Render_Markdig(Shape=Blocks)",
            "value": 5721.717,
            "range": "± 25.959",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080636\nmean 5720.439 ns"
          },
          {
            "name": "MarkdownShapeBenchmarks.Render(Shape=Inline)",
            "value": 8027.915,
            "range": "± 77.304",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080636\nmean 8019.283 ns"
          },
          {
            "name": "MarkdownShapeBenchmarks.Render_Markdig(Shape=Inline)",
            "value": 4605.672,
            "range": "± 32.895",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080636\nmean 4599.847 ns"
          },
          {
            "name": "MediaSampleBenchmarks.GetSample",
            "value": 184.755,
            "range": "± 0.269",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080636\nmean 184.774 ns"
          },
          {
            "name": "MediaSampleBenchmarks.FindSample",
            "value": 2089.179,
            "range": "± 1.022",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080636\nmean 2089.475 ns"
          },
          {
            "name": "MediaSampleBenchmarks.Seek",
            "value": 4791.434,
            "range": "± 3.994",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080636\nmean 4790.931 ns"
          },
          {
            "name": "MkvBlockBenchmarks.Parse(Lacing=Xiph)",
            "value": 204.449,
            "range": "± 0.975",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080636\nmean 204.404 ns"
          },
          {
            "name": "MkvBlockBenchmarks.Parse(Lacing=FixedSize)",
            "value": 181.427,
            "range": "± 1.253",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080636\nmean 181.376 ns"
          },
          {
            "name": "MkvBlockBenchmarks.Parse(Lacing=Ebml)",
            "value": 222.372,
            "range": "± 1.726",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080636\nmean 222.407 ns"
          },
          {
            "name": "MkvDocumentBenchmarks.Load",
            "value": 67533.229,
            "range": "± 725.398",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080636\nmean 67410.878 ns"
          },
          {
            "name": "MkvDocumentBenchmarks.ReadSamples",
            "value": 3869.504,
            "range": "± 16.755",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080636\nmean 3871.925 ns"
          },
          {
            "name": "Mp4FileBenchmarks.Load",
            "value": 36661.07,
            "range": "± 522.563",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080636\nmean 36864.931 ns"
          },
          {
            "name": "PdfPageBenchmarks.ReadGeometry(Pages=1)",
            "value": 14057.628,
            "range": "± 82.586",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080636\nmean 14079.68 ns"
          },
          {
            "name": "PdfPageBenchmarks.ReadGeometry_PdfPig(Pages=1)",
            "value": 1719288.282,
            "range": "± 593812.932",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080636\nmean 1611918.115 ns"
          },
          {
            "name": "PdfPageBenchmarks.ReadGeometry(Pages=8)",
            "value": 34572,
            "range": "± 184.767",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080636\nmean 34561.777 ns"
          },
          {
            "name": "PdfPageBenchmarks.ReadGeometry_PdfPig(Pages=8)",
            "value": 1730822.253,
            "range": "± 540979.676",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080636\nmean 1594533.02 ns"
          },
          {
            "name": "TextParsingBenchmarks.DecodeDetected",
            "value": 1884.208,
            "range": "± 19.716",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080636\nmean 1887.457 ns"
          },
          {
            "name": "TextParsingBenchmarks.ReadLines",
            "value": 8528.929,
            "range": "± 91.599",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080636\nmean 8501.607 ns"
          },
          {
            "name": "TextParsingBenchmarks.Tokenize",
            "value": 70135.434,
            "range": "± 161.049",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080636\nmean 70167.152 ns"
          },
          {
            "name": "YamlBenchmarks.Parse_YamlDotNet",
            "value": 56080.173,
            "range": "± 138.454",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080636\nmean 56117.355 ns"
          },
          {
            "name": "YamlBenchmarks.Parse",
            "value": 20225.954,
            "range": "± 126.65",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080636\nmean 20224 ns"
          },
          {
            "name": "YamlEventBenchmarks.ParseEvents(Shape=Flow)",
            "value": 22031.799,
            "range": "± 114.472",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080636\nmean 22042.89 ns"
          },
          {
            "name": "YamlEventBenchmarks.ParseEvents_YamlDotNet(Shape=Flow)",
            "value": 71466.281,
            "range": "± 265.453",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080636\nmean 71566.484 ns"
          },
          {
            "name": "YamlEventBenchmarks.ParseEvents(Shape=Scalars)",
            "value": 23417.233,
            "range": "± 120.362",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080636\nmean 23383.545 ns"
          },
          {
            "name": "YamlEventBenchmarks.ParseEvents_YamlDotNet(Shape=Scalars)",
            "value": 70546.573,
            "range": "± 303.66",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080636\nmean 70636.622 ns"
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
          "id": "699980c0cf28b76854e64dc51e4e45d2c5c05dee",
          "message": "Merge pull request #8 from assimalign/feature/benchmark-suite-expansion\n\nBuild out the benchmark suites: 148 to 988 cases across every area but Database",
          "timestamp": "2026-10-20T07:59:35Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/699980c0cf28b76854e64dc51e4e45d2c5c05dee"
        },
        "date": 1792485119565,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "BinaryReaderBenchmarks.ReadRecords(Records=8) - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080636"
          },
          {
            "name": "BinaryReaderBenchmarks.WriteRecords(Records=8) - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080636"
          },
          {
            "name": "BinaryReaderBenchmarks.ReadRecords(Records=128) - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080636"
          },
          {
            "name": "BinaryReaderBenchmarks.WriteRecords(Records=128) - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080636"
          },
          {
            "name": "BmffCursorBenchmarks.WalkHeaders - allocated",
            "value": 7784,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080636"
          },
          {
            "name": "BmffDocumentBenchmarks.Load - allocated",
            "value": 19168,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080636"
          },
          {
            "name": "CompositeContentBenchmarks.ReadParts - allocated",
            "value": 3064,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080636"
          },
          {
            "name": "EbmlReaderBenchmarks.ReadValues - allocated",
            "value": 4840,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080636"
          },
          {
            "name": "EbmlReaderBenchmarks.Load - allocated",
            "value": 11480,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080636"
          },
          {
            "name": "ExecutableReaderBenchmarks.Read(Format=ELF) - allocated",
            "value": 496,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080636"
          },
          {
            "name": "ExecutableReaderBenchmarks.Read(Format=MachO) - allocated",
            "value": 408,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080636"
          },
          {
            "name": "ExecutableReaderBenchmarks.Read(Format=PE) - allocated",
            "value": 664,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080636"
          },
          {
            "name": "HeifDocumentBenchmarks.Load(Brand=avif) - allocated",
            "value": 41976,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080636"
          },
          {
            "name": "HeifDocumentBenchmarks.Load(Brand=heic) - allocated",
            "value": 41976,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080636"
          },
          {
            "name": "MarkdownBenchmarks.Parse_Markdig - allocated",
            "value": 19984,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080636"
          },
          {
            "name": "MarkdownBenchmarks.Parse - allocated",
            "value": 128745,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080636"
          },
          {
            "name": "MarkdownBenchmarks.ToHtml_Markdig - allocated",
            "value": 24224,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080636"
          },
          {
            "name": "MarkdownBenchmarks.ToHtml - allocated",
            "value": 148753,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080636"
          },
          {
            "name": "MarkdownShapeBenchmarks.Parse(Shape=Blocks) - allocated",
            "value": 238449,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080636"
          },
          {
            "name": "MarkdownShapeBenchmarks.Parse_Markdig(Shape=Blocks) - allocated",
            "value": 32120,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080636"
          },
          {
            "name": "MarkdownShapeBenchmarks.Parse(Shape=Inline) - allocated",
            "value": 139569,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080636"
          },
          {
            "name": "MarkdownShapeBenchmarks.Parse_Markdig(Shape=Inline) - allocated",
            "value": 27752,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080636"
          },
          {
            "name": "MarkdownShapeBenchmarks.Render(Shape=Blocks) - allocated",
            "value": 24792,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080636"
          },
          {
            "name": "MarkdownShapeBenchmarks.Render_Markdig(Shape=Blocks) - allocated",
            "value": 4248,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080636"
          },
          {
            "name": "MarkdownShapeBenchmarks.Render(Shape=Inline) - allocated",
            "value": 16776,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080636"
          },
          {
            "name": "MarkdownShapeBenchmarks.Render_Markdig(Shape=Inline) - allocated",
            "value": 3336,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080636"
          },
          {
            "name": "MediaSampleBenchmarks.GetSample - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080636"
          },
          {
            "name": "MediaSampleBenchmarks.FindSample - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080636"
          },
          {
            "name": "MediaSampleBenchmarks.Seek - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080636"
          },
          {
            "name": "MkvBlockBenchmarks.Parse(Lacing=Xiph) - allocated",
            "value": 456,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080636"
          },
          {
            "name": "MkvBlockBenchmarks.Parse(Lacing=FixedSize) - allocated",
            "value": 456,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080636"
          },
          {
            "name": "MkvBlockBenchmarks.Parse(Lacing=Ebml) - allocated",
            "value": 456,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080636"
          },
          {
            "name": "MkvDocumentBenchmarks.Load - allocated",
            "value": 101721,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080636"
          },
          {
            "name": "MkvDocumentBenchmarks.ReadSamples - allocated",
            "value": 6008,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080636"
          },
          {
            "name": "Mp4FileBenchmarks.Load - allocated",
            "value": 58144,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080636"
          },
          {
            "name": "PdfPageBenchmarks.ReadGeometry(Pages=1) - allocated",
            "value": 18688,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080636"
          },
          {
            "name": "PdfPageBenchmarks.ReadGeometry_PdfPig(Pages=1) - allocated",
            "value": 2137101,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080636"
          },
          {
            "name": "PdfPageBenchmarks.ReadGeometry(Pages=8) - allocated",
            "value": 44360,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080636"
          },
          {
            "name": "PdfPageBenchmarks.ReadGeometry_PdfPig(Pages=8) - allocated",
            "value": 2163384,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080636"
          },
          {
            "name": "TextParsingBenchmarks.DecodeDetected - allocated",
            "value": 16832,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080636"
          },
          {
            "name": "TextParsingBenchmarks.ReadLines - allocated",
            "value": 5504,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080636"
          },
          {
            "name": "TextParsingBenchmarks.Tokenize - allocated",
            "value": 2553,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080636"
          },
          {
            "name": "YamlBenchmarks.Parse_YamlDotNet - allocated",
            "value": 55928,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080636"
          },
          {
            "name": "YamlBenchmarks.Parse - allocated",
            "value": 46072,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080636"
          },
          {
            "name": "YamlEventBenchmarks.ParseEvents(Shape=Flow) - allocated",
            "value": 68792,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080636"
          },
          {
            "name": "YamlEventBenchmarks.ParseEvents_YamlDotNet(Shape=Flow) - allocated",
            "value": 67977,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080636"
          },
          {
            "name": "YamlEventBenchmarks.ParseEvents(Shape=Scalars) - allocated",
            "value": 61624,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080636"
          },
          {
            "name": "YamlEventBenchmarks.ParseEvents_YamlDotNet(Shape=Scalars) - allocated",
            "value": 54537,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080636"
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
          "id": "699980c0cf28b76854e64dc51e4e45d2c5c05dee",
          "message": "Merge pull request #8 from assimalign/feature/benchmark-suite-expansion\n\nBuild out the benchmark suites: 148 to 988 cases across every area but Database",
          "timestamp": "2026-10-20T07:59:35Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/699980c0cf28b76854e64dc51e4e45d2c5c05dee"
        },
        "date": 1792485135579,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "FileSystemPathBenchmarks.GetSegments",
            "value": 110.956,
            "range": "± 2.992",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080640\nmean 111.528 ns"
          },
          {
            "name": "FileSystemPathBenchmarks.ParseSpan",
            "value": 1113.936,
            "range": "± 22.01",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080640\nmean 1106.265 ns"
          },
          {
            "name": "FileSystemPathBenchmarks.Subpath",
            "value": 100.879,
            "range": "± 1.378",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080640\nmean 101.225 ns"
          },
          {
            "name": "FileSystemPathBenchmarks.Merge",
            "value": 1874.461,
            "range": "± 21.129",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080640\nmean 1871.173 ns"
          },
          {
            "name": "FileSystemPathBenchmarks.Parse(value=//tes(...)e.txt [33])",
            "value": 1028.283,
            "range": "± 10.281",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080640\nmean 1028.189 ns"
          },
          {
            "name": "FileSystemPathBenchmarks.Parse(value=C:/te(...)e.txt [34])",
            "value": 1125.205,
            "range": "± 16.414",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080640\nmean 1123.403 ns"
          },
          {
            "name": "GlobBenchmarks.IsMatch_Regex(Pattern=p?th/(...)].txt [21])",
            "value": 1089.457,
            "range": "± 4.44",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080640\nmean 1089.108 ns"
          },
          {
            "name": "GlobBenchmarks.IsMatch(Pattern=p?th/(...)].txt [21])",
            "value": 2749.303,
            "range": "± 6.141",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080640\nmean 2748.945 ns"
          },
          {
            "name": "GlobBenchmarks.IsMatch_Regex(Pattern=src/**/*.{cs,json})",
            "value": 1968.542,
            "range": "± 2.961",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080640\nmean 1969.118 ns"
          },
          {
            "name": "GlobBenchmarks.IsMatch(Pattern=src/**/*.{cs,json})",
            "value": 4749.748,
            "range": "± 6.584",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080640\nmean 4751.727 ns"
          },
          {
            "name": "MemoiseBenchmarks.CachedTransform",
            "value": 301.156,
            "range": "± 0.886",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080640\nmean 301.318 ns"
          },
          {
            "name": "MemoiseBenchmarks.CachedTransform_ConcurrentDictionary",
            "value": 80.486,
            "range": "± 0.24",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080640\nmean 80.512 ns"
          },
          {
            "name": "MemoryPoolBenchmarks.RentAndReturn(Length=256)",
            "value": 118.029,
            "range": "± 2.4",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080640\nmean 118.415 ns"
          },
          {
            "name": "MemoryPoolBenchmarks.RentAndReturn_MemoryPool(Length=256)",
            "value": 11.886,
            "range": "± 0.254",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080640\nmean 11.875 ns"
          },
          {
            "name": "MemoryPoolBenchmarks.RentAndReturn(Length=4096)",
            "value": 143.85,
            "range": "± 0.975",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080640\nmean 144.14 ns"
          },
          {
            "name": "MemoryPoolBenchmarks.RentAndReturn_MemoryPool(Length=4096)",
            "value": 32.21,
            "range": "± 0.193",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080640\nmean 32.224 ns"
          },
          {
            "name": "StringFilterBenchmarks.StartsWithAny(Match=False)",
            "value": 234.316,
            "range": "± 1.945",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080640\nmean 234.596 ns"
          },
          {
            "name": "StringFilterBenchmarks.StartsWithAny_String(Match=False)",
            "value": 220.772,
            "range": "± 1.879",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080640\nmean 221.196 ns"
          },
          {
            "name": "StringFilterBenchmarks.StartsWithAny(Match=True)",
            "value": 278.634,
            "range": "± 6.899",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080640\nmean 281.349 ns"
          },
          {
            "name": "StringFilterBenchmarks.StartsWithAny_String(Match=True)",
            "value": 264.052,
            "range": "± 2.925",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080640\nmean 264.124 ns"
          },
          {
            "name": "StringFilterBenchmarks.EndsWithAny(Match=False)",
            "value": 577.564,
            "range": "± 10.499",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080640\nmean 580.659 ns"
          },
          {
            "name": "StringFilterBenchmarks.EndsWithAny_String(Match=False)",
            "value": 564.709,
            "range": "± 6.265",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080640\nmean 564.085 ns"
          },
          {
            "name": "StringFilterBenchmarks.EndsWithAny(Match=True)",
            "value": 566.623,
            "range": "± 4.597",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080640\nmean 567.755 ns"
          },
          {
            "name": "StringFilterBenchmarks.EndsWithAny_String(Match=True)",
            "value": 547.843,
            "range": "± 6.681",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080640\nmean 547.407 ns"
          },
          {
            "name": "UlidBenchmarks.Parse",
            "value": 16.315,
            "range": "± 0.339",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080640\nmean 16.379 ns"
          },
          {
            "name": "UlidBenchmarks.ParseSpan",
            "value": 16.216,
            "range": "± 0.17",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080640\nmean 16.253 ns"
          },
          {
            "name": "UlidBenchmarks.ParseUtf8",
            "value": 17.895,
            "range": "± 0.217",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080640\nmean 17.891 ns"
          },
          {
            "name": "UlidBenchmarks.RejectInvalid",
            "value": 7.834,
            "range": "± 0.316",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080640\nmean 7.785 ns"
          },
          {
            "name": "UlidBenchmarks.Format",
            "value": 12.22,
            "range": "± 0.447",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080640\nmean 12.369 ns"
          },
          {
            "name": "UlidBenchmarks.FormatCharacters",
            "value": 6.994,
            "range": "± 0.044",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080640\nmean 6.989 ns"
          },
          {
            "name": "UlidBenchmarks.FormatUtf8",
            "value": 7.03,
            "range": "± 0.082",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080640\nmean 7.027 ns"
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
          "id": "699980c0cf28b76854e64dc51e4e45d2c5c05dee",
          "message": "Merge pull request #8 from assimalign/feature/benchmark-suite-expansion\n\nBuild out the benchmark suites: 148 to 988 cases across every area but Database",
          "timestamp": "2026-10-20T07:59:35Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/699980c0cf28b76854e64dc51e4e45d2c5c05dee"
        },
        "date": 1792485138977,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "FileSystemPathBenchmarks.GetSegments - allocated",
            "value": 520,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080640"
          },
          {
            "name": "FileSystemPathBenchmarks.ParseSpan - allocated",
            "value": 4272,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080640"
          },
          {
            "name": "FileSystemPathBenchmarks.Subpath - allocated",
            "value": 424,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080640"
          },
          {
            "name": "FileSystemPathBenchmarks.Merge - allocated",
            "value": 7256,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080640"
          },
          {
            "name": "FileSystemPathBenchmarks.Parse(value=//tes(...)e.txt [33]) - allocated",
            "value": 3904,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080640"
          },
          {
            "name": "FileSystemPathBenchmarks.Parse(value=C:/te(...)e.txt [34]) - allocated",
            "value": 4272,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080640"
          },
          {
            "name": "GlobBenchmarks.IsMatch_Regex(Pattern=p?th/(...)].txt [21]) - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080640"
          },
          {
            "name": "GlobBenchmarks.IsMatch(Pattern=p?th/(...)].txt [21]) - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080640"
          },
          {
            "name": "GlobBenchmarks.IsMatch_Regex(Pattern=src/**/*.{cs,json}) - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080640"
          },
          {
            "name": "GlobBenchmarks.IsMatch(Pattern=src/**/*.{cs,json}) - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080640"
          },
          {
            "name": "MemoiseBenchmarks.CachedTransform - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080640"
          },
          {
            "name": "MemoiseBenchmarks.CachedTransform_ConcurrentDictionary - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080640"
          },
          {
            "name": "MemoryPoolBenchmarks.RentAndReturn(Length=256) - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080640"
          },
          {
            "name": "MemoryPoolBenchmarks.RentAndReturn_MemoryPool(Length=256) - allocated",
            "value": 24,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080640"
          },
          {
            "name": "MemoryPoolBenchmarks.RentAndReturn(Length=4096) - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080640"
          },
          {
            "name": "MemoryPoolBenchmarks.RentAndReturn_MemoryPool(Length=4096) - allocated",
            "value": 24,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080640"
          },
          {
            "name": "StringFilterBenchmarks.StartsWithAny(Match=False) - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080640"
          },
          {
            "name": "StringFilterBenchmarks.StartsWithAny_String(Match=False) - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080640"
          },
          {
            "name": "StringFilterBenchmarks.StartsWithAny(Match=True) - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080640"
          },
          {
            "name": "StringFilterBenchmarks.StartsWithAny_String(Match=True) - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080640"
          },
          {
            "name": "StringFilterBenchmarks.EndsWithAny(Match=False) - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080640"
          },
          {
            "name": "StringFilterBenchmarks.EndsWithAny_String(Match=False) - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080640"
          },
          {
            "name": "StringFilterBenchmarks.EndsWithAny(Match=True) - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080640"
          },
          {
            "name": "StringFilterBenchmarks.EndsWithAny_String(Match=True) - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080640"
          },
          {
            "name": "UlidBenchmarks.Parse - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080640"
          },
          {
            "name": "UlidBenchmarks.ParseSpan - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080640"
          },
          {
            "name": "UlidBenchmarks.ParseUtf8 - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080640"
          },
          {
            "name": "UlidBenchmarks.RejectInvalid - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080640"
          },
          {
            "name": "UlidBenchmarks.Format - allocated",
            "value": 80,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080640"
          },
          {
            "name": "UlidBenchmarks.FormatCharacters - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080640"
          },
          {
            "name": "UlidBenchmarks.FormatUtf8 - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080640"
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
        "date": 1792448945377,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "OtlpLogExporterBenchmarks.ExportAsync(BatchSize=1)",
            "value": 3381.585,
            "range": "± 15.068",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080384\nmean 3379.204 ns"
          },
          {
            "name": "OtlpLogExporterBenchmarks.EnqueueThenFlush(BatchSize=1)",
            "value": 3929.526,
            "range": "± 31.69",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080384\nmean 3930.909 ns"
          },
          {
            "name": "OtlpLogExporterBenchmarks.ExportAsync(BatchSize=64)",
            "value": 96762.119,
            "range": "± 533.453",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080384\nmean 96748.607 ns"
          },
          {
            "name": "OtlpLogExporterBenchmarks.EnqueueThenFlush(BatchSize=64)",
            "value": 98156.414,
            "range": "± 962.416",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080384\nmean 97955.074 ns"
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
        "date": 1792448948264,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "OtlpLogExporterBenchmarks.ExportAsync(BatchSize=1) - allocated",
            "value": 4616,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080384"
          },
          {
            "name": "OtlpLogExporterBenchmarks.EnqueueThenFlush(BatchSize=1) - allocated",
            "value": 8736,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080384"
          },
          {
            "name": "OtlpLogExporterBenchmarks.ExportAsync(BatchSize=64) - allocated",
            "value": 95296,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080384"
          },
          {
            "name": "OtlpLogExporterBenchmarks.EnqueueThenFlush(BatchSize=64) - allocated",
            "value": 99416,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080384"
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
        "date": 1792448963036,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "CircuitBreakerPipelineBenchmarks.Execute_Polly",
            "value": 208.891,
            "range": "± 1.04",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080390\nmean 209.246 ns"
          },
          {
            "name": "CircuitBreakerPipelineBenchmarks.Execute",
            "value": 269.531,
            "range": "± 0.966",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080390\nmean 269.458 ns"
          },
          {
            "name": "RetryPipelineBenchmarks.Execute_Polly",
            "value": 194.103,
            "range": "± 0.16",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080390\nmean 194.106 ns"
          },
          {
            "name": "RetryPipelineBenchmarks.Execute",
            "value": 243.767,
            "range": "± 0.781",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080390\nmean 243.975 ns"
          },
          {
            "name": "TimeoutPipelineBenchmarks.Execute_Polly",
            "value": 203.222,
            "range": "± 0.272",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080390\nmean 203.284 ns"
          },
          {
            "name": "TimeoutPipelineBenchmarks.Execute",
            "value": 1040.538,
            "range": "± 211.115",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080390\nmean 1115.713 ns"
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
        "date": 1792448965363,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "CircuitBreakerPipelineBenchmarks.Execute_Polly - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080390"
          },
          {
            "name": "CircuitBreakerPipelineBenchmarks.Execute - allocated",
            "value": 200,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080390"
          },
          {
            "name": "RetryPipelineBenchmarks.Execute_Polly - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080390"
          },
          {
            "name": "RetryPipelineBenchmarks.Execute - allocated",
            "value": 112,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080390"
          },
          {
            "name": "TimeoutPipelineBenchmarks.Execute_Polly - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080390"
          },
          {
            "name": "TimeoutPipelineBenchmarks.Execute - allocated",
            "value": 560,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080390"
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
        "date": 1792448982680,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "DataProtectorBenchmarks.Protect_AspNetCore(PayloadSize=64)",
            "value": 7096.641,
            "range": "± 78.695",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080394\nmean 7106.456 ns"
          },
          {
            "name": "DataProtectorBenchmarks.Protect(PayloadSize=64)",
            "value": 4230.89,
            "range": "± 80.095",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080394\nmean 4254.746 ns"
          },
          {
            "name": "DataProtectorBenchmarks.Protect_AspNetCore(PayloadSize=1024)",
            "value": 8460.887,
            "range": "± 127.312",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080394\nmean 8460.185 ns"
          },
          {
            "name": "DataProtectorBenchmarks.Protect(PayloadSize=1024)",
            "value": 4282.167,
            "range": "± 39.134",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080394\nmean 4287.482 ns"
          },
          {
            "name": "DataProtectorBenchmarks.Unprotect_AspNetCore(PayloadSize=64)",
            "value": 5415.027,
            "range": "± 111.828",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080394\nmean 5456.694 ns"
          },
          {
            "name": "DataProtectorBenchmarks.Unprotect(PayloadSize=64)",
            "value": 3496.895,
            "range": "± 40.095",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080394\nmean 3484.838 ns"
          },
          {
            "name": "DataProtectorBenchmarks.Unprotect_AspNetCore(PayloadSize=1024)",
            "value": 6377.136,
            "range": "± 106.885",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080394\nmean 6385.538 ns"
          },
          {
            "name": "DataProtectorBenchmarks.Unprotect(PayloadSize=1024)",
            "value": 3699.497,
            "range": "± 94.195",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080394\nmean 3702.151 ns"
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
        "date": 1792448985354,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "DataProtectorBenchmarks.Protect_AspNetCore(PayloadSize=64) - allocated",
            "value": 1192,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080394"
          },
          {
            "name": "DataProtectorBenchmarks.Protect(PayloadSize=64) - allocated",
            "value": 168,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080394"
          },
          {
            "name": "DataProtectorBenchmarks.Protect_AspNetCore(PayloadSize=1024) - allocated",
            "value": 3112,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080394"
          },
          {
            "name": "DataProtectorBenchmarks.Protect(PayloadSize=1024) - allocated",
            "value": 1128,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080394"
          },
          {
            "name": "DataProtectorBenchmarks.Unprotect_AspNetCore(PayloadSize=64) - allocated",
            "value": 920,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080394"
          },
          {
            "name": "DataProtectorBenchmarks.Unprotect(PayloadSize=64) - allocated",
            "value": 120,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080394"
          },
          {
            "name": "DataProtectorBenchmarks.Unprotect_AspNetCore(PayloadSize=1024) - allocated",
            "value": 1880,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080394"
          },
          {
            "name": "DataProtectorBenchmarks.Unprotect(PayloadSize=1024) - allocated",
            "value": 1080,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080394"
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
        "date": 1792449000218,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "KeyValuePointBenchmarks.Get",
            "value": 2768.599,
            "range": "± 42.799",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080398\nmean 2783.392 ns"
          },
          {
            "name": "SqlQueryParserBenchmarks.Parse(Statement=Insert)",
            "value": 12445.723,
            "range": "± 38.652",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080398\nmean 12455.492 ns"
          },
          {
            "name": "SqlQueryParserBenchmarks.Parse(Statement=Join)",
            "value": 41948.389,
            "range": "± 71.205",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080398\nmean 41955.744 ns"
          },
          {
            "name": "SqlQueryParserBenchmarks.Parse(Statement=PointSelect)",
            "value": 12263.01,
            "range": "± 86.803",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080398\nmean 12273.77 ns"
          },
          {
            "name": "SqlPointQueryBenchmarks.SelectById_Sqlite",
            "value": 3251.856,
            "range": "± 17.367",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080398\nmean 3257.869 ns"
          },
          {
            "name": "SqlPointQueryBenchmarks.SelectById",
            "value": 14327.642,
            "range": "± 108.293",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080398\nmean 14350.07 ns"
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
        "date": 1792449002575,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "KeyValuePointBenchmarks.Get - allocated",
            "value": 2449,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080398"
          },
          {
            "name": "SqlQueryParserBenchmarks.Parse(Statement=Insert) - allocated",
            "value": 4928,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080398"
          },
          {
            "name": "SqlQueryParserBenchmarks.Parse(Statement=Join) - allocated",
            "value": 13048,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080398"
          },
          {
            "name": "SqlQueryParserBenchmarks.Parse(Statement=PointSelect) - allocated",
            "value": 4832,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080398"
          },
          {
            "name": "SqlPointQueryBenchmarks.SelectById_Sqlite - allocated",
            "value": 1040,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080398"
          },
          {
            "name": "SqlPointQueryBenchmarks.SelectById - allocated",
            "value": 10000,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080398"
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
        "date": 1792449018942,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "NatGatewayApplicationBenchmarks.BuildStartStop(ServiceCount=1)",
            "value": 3553.54,
            "range": "± 18.655",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080403\nmean 3550.79 ns"
          },
          {
            "name": "NatGatewayApplicationBenchmarks.BuildStartStop(ServiceCount=16)",
            "value": 9397.854,
            "range": "± 49.208",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080403\nmean 9390.915 ns"
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
        "date": 1792449020755,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "NatGatewayApplicationBenchmarks.BuildStartStop(ServiceCount=1) - allocated",
            "value": 7864,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080403"
          },
          {
            "name": "NatGatewayApplicationBenchmarks.BuildStartStop(ServiceCount=16) - allocated",
            "value": 20952,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080403"
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
        "date": 1792449034338,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "CrontabBenchmarks.NextOccurrence_Cronos(Expression=*/5 * * * *)",
            "value": 31.491,
            "range": "± 0.055",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080405\nmean 31.493 ns"
          },
          {
            "name": "CrontabBenchmarks.NextOccurrence(Expression=*/5 * * * *)",
            "value": 251.558,
            "range": "± 1.758",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080405\nmean 251.847 ns"
          },
          {
            "name": "CrontabBenchmarks.NextOccurrence_Cronos(Expression=0 9 * * 1-5)",
            "value": 71.632,
            "range": "± 0.131",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080405\nmean 71.678 ns"
          },
          {
            "name": "CrontabBenchmarks.NextOccurrence(Expression=0 9 * * 1-5)",
            "value": 1207.91,
            "range": "± 2.098",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080405\nmean 1207.813 ns"
          },
          {
            "name": "CrontabBenchmarks.Parse_Cronos(Expression=*/5 * * * *)",
            "value": 40.568,
            "range": "± 0.345",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080405\nmean 40.539 ns"
          },
          {
            "name": "CrontabBenchmarks.Parse(Expression=*/5 * * * *)",
            "value": 3631.009,
            "range": "± 64.946",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080405\nmean 3636.027 ns"
          },
          {
            "name": "CrontabBenchmarks.Parse_Cronos(Expression=0 9 * * 1-5)",
            "value": 56.37,
            "range": "± 0.44",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080405\nmean 56.462 ns"
          },
          {
            "name": "CrontabBenchmarks.Parse(Expression=0 9 * * 1-5)",
            "value": 2497.619,
            "range": "± 21.536",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080405\nmean 2500.257 ns"
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
        "date": 1792449036762,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "CrontabBenchmarks.NextOccurrence_Cronos(Expression=*/5 * * * *) - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080405"
          },
          {
            "name": "CrontabBenchmarks.NextOccurrence(Expression=*/5 * * * *) - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080405"
          },
          {
            "name": "CrontabBenchmarks.NextOccurrence_Cronos(Expression=0 9 * * 1-5) - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080405"
          },
          {
            "name": "CrontabBenchmarks.NextOccurrence(Expression=0 9 * * 1-5) - allocated",
            "value": 0,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080405"
          },
          {
            "name": "CrontabBenchmarks.Parse_Cronos(Expression=*/5 * * * *) - allocated",
            "value": 48,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080405"
          },
          {
            "name": "CrontabBenchmarks.Parse(Expression=*/5 * * * *) - allocated",
            "value": 6272,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080405"
          },
          {
            "name": "CrontabBenchmarks.Parse_Cronos(Expression=0 9 * * 1-5) - allocated",
            "value": 48,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080405"
          },
          {
            "name": "CrontabBenchmarks.Parse(Expression=0 9 * * 1-5) - allocated",
            "value": 4656,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080405"
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
        "date": 1792449053188,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "HttpRoundTripBenchmarks.Get(Target=nginx)",
            "value": 190238.549,
            "range": "± 687.199",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080410\nmean 190207.924 ns"
          },
          {
            "name": "HttpRoundTripBenchmarks.Get(Target=node)",
            "value": 64344.775,
            "range": "± 4897.57",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080410\nmean 64728.902 ns"
          },
          {
            "name": "HttpRoundTripBenchmarks.Get(Target=python)",
            "value": 106721.296,
            "range": "± 5700.962",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080410\nmean 107591.364 ns"
          },
          {
            "name": "ProcessStartupBenchmarks.StartUntilReady(server=node)",
            "value": 87073969,
            "range": "± 3173195.37",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080410\nmean 87810723.95 ns"
          },
          {
            "name": "ProcessStartupBenchmarks.RunToExit(command=node-run)",
            "value": 23200547.5,
            "range": "± 1260439.764",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080410\nmean 23398001.05 ns"
          },
          {
            "name": "ProcessStartupBenchmarks.StartUntilReady(server=python)",
            "value": 80590733,
            "range": "± 1273467.666",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080410\nmean 80922712 ns"
          },
          {
            "name": "JwtBearerAuthenticationBenchmarks.Authenticate_AspNetCore",
            "value": 7946.76,
            "range": "± 62.5",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080410\nmean 7953.477 ns"
          },
          {
            "name": "JwtBearerAuthenticationBenchmarks.Authenticate",
            "value": 6930.192,
            "range": "± 65.057",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080410\nmean 6951.269 ns"
          },
          {
            "name": "CookieAuthenticationBenchmarks.Authenticate_AspNetCore",
            "value": 15982.942,
            "range": "± 70.401",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080410\nmean 16001.106 ns"
          },
          {
            "name": "CookieAuthenticationBenchmarks.Authenticate",
            "value": 8801.299,
            "range": "± 59.802",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080410\nmean 8805.013 ns"
          },
          {
            "name": "RolePolicyBenchmarks.Evaluate_AspNetCore",
            "value": 467.737,
            "range": "± 4.507",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080410\nmean 468.621 ns"
          },
          {
            "name": "RolePolicyBenchmarks.Evaluate",
            "value": 76.72,
            "range": "± 0.318",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080410\nmean 76.742 ns"
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
        "date": 1792449056386,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "HttpRoundTripBenchmarks.Get(Target=nginx) - allocated",
            "value": 74,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080410"
          },
          {
            "name": "HttpRoundTripBenchmarks.Get(Target=node) - allocated",
            "value": 64,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080410"
          },
          {
            "name": "HttpRoundTripBenchmarks.Get(Target=python) - allocated",
            "value": 66,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080410"
          },
          {
            "name": "ProcessStartupBenchmarks.StartUntilReady(server=node) - allocated",
            "value": 2389024,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080410"
          },
          {
            "name": "ProcessStartupBenchmarks.RunToExit(command=node-run) - allocated",
            "value": 95928,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080410"
          },
          {
            "name": "ProcessStartupBenchmarks.StartUntilReady(server=python) - allocated",
            "value": 2376200,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080410"
          },
          {
            "name": "JwtBearerAuthenticationBenchmarks.Authenticate_AspNetCore - allocated",
            "value": 12256,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080410"
          },
          {
            "name": "JwtBearerAuthenticationBenchmarks.Authenticate - allocated",
            "value": 10712,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080410"
          },
          {
            "name": "CookieAuthenticationBenchmarks.Authenticate_AspNetCore - allocated",
            "value": 7744,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080410"
          },
          {
            "name": "CookieAuthenticationBenchmarks.Authenticate - allocated",
            "value": 5936,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080410"
          },
          {
            "name": "RolePolicyBenchmarks.Evaluate_AspNetCore - allocated",
            "value": 704,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080410"
          },
          {
            "name": "RolePolicyBenchmarks.Evaluate - allocated",
            "value": 72,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.44 (179db17d132b) on GitHub Actions 1000080410"
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
          "id": "699980c0cf28b76854e64dc51e4e45d2c5c05dee",
          "message": "Merge pull request #8 from assimalign/feature/benchmark-suite-expansion\n\nBuild out the benchmark suites: 148 to 988 cases across every area but Database",
          "timestamp": "2026-10-20T07:59:35Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/699980c0cf28b76854e64dc51e4e45d2c5c05dee"
        },
        "date": 1792485029514,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "AmqpDataMessageBenchmarks.Decode(PayloadSize=64)",
            "value": 68.192,
            "range": "± 1.151",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080622\nmean 68.259 ns"
          },
          {
            "name": "AmqpDataMessageBenchmarks.Decode_AmqpNetLite(PayloadSize=64)",
            "value": 58.083,
            "range": "± 0.693",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080622\nmean 58.173 ns"
          },
          {
            "name": "AmqpDataMessageBenchmarks.Decode(PayloadSize=16384)",
            "value": 1078.322,
            "range": "± 26.391",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080622\nmean 1073.149 ns"
          },
          {
            "name": "AmqpDataMessageBenchmarks.Decode_AmqpNetLite(PayloadSize=16384)",
            "value": 1079.371,
            "range": "± 23.913",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080622\nmean 1074.199 ns"
          },
          {
            "name": "AmqpDataMessageBenchmarks.Encode(PayloadSize=64)",
            "value": 86.284,
            "range": "± 1.943",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080622\nmean 86.626 ns"
          },
          {
            "name": "AmqpDataMessageBenchmarks.Encode_AmqpNetLite(PayloadSize=64)",
            "value": 39.32,
            "range": "± 0.546",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080622\nmean 39.412 ns"
          },
          {
            "name": "AmqpDataMessageBenchmarks.Encode(PayloadSize=16384)",
            "value": 2057.276,
            "range": "± 29.747",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080622\nmean 2052.248 ns"
          },
          {
            "name": "AmqpDataMessageBenchmarks.Encode_AmqpNetLite(PayloadSize=16384)",
            "value": 986.673,
            "range": "± 66.015",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080622\nmean 1015.933 ns"
          },
          {
            "name": "AmqpDeliveryFrameBenchmarks.Encode(Kind=Credit)",
            "value": 338.255,
            "range": "± 3.368",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080622\nmean 338.959 ns"
          },
          {
            "name": "AmqpDeliveryFrameBenchmarks.Decode(Kind=Credit)",
            "value": 160.465,
            "range": "± 1.73",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080622\nmean 160.838 ns"
          },
          {
            "name": "AmqpDeliveryFrameBenchmarks.Encode(Kind=Disposition)",
            "value": 341.638,
            "range": "± 4.125",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080622\nmean 342.779 ns"
          },
          {
            "name": "AmqpDeliveryFrameBenchmarks.Decode(Kind=Disposition)",
            "value": 154.714,
            "range": "± 0.834",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080622\nmean 154.862 ns"
          },
          {
            "name": "AmqpDeliveryFrameBenchmarks.Encode(Kind=Drain)",
            "value": 582.709,
            "range": "± 4.446",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080622\nmean 581.14 ns"
          },
          {
            "name": "AmqpDeliveryFrameBenchmarks.Decode(Kind=Drain)",
            "value": 493.002,
            "range": "± 8.011",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080622\nmean 489.49 ns"
          },
          {
            "name": "AmqpFrameBenchmarks.EncodeTransferFrame",
            "value": 505.348,
            "range": "± 10.463",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080622\nmean 506.311 ns"
          },
          {
            "name": "AmqpFrameBenchmarks.DecodeTransferFrame",
            "value": 191.979,
            "range": "± 5.001",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080622\nmean 190.959 ns"
          },
          {
            "name": "AmqpMessageBenchmarks.Decode_AmqpNetLite",
            "value": 592.886,
            "range": "± 4.78",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080622\nmean 592.239 ns"
          },
          {
            "name": "AmqpMessageBenchmarks.Decode",
            "value": 782.938,
            "range": "± 4.916",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080622\nmean 783.563 ns"
          },
          {
            "name": "AmqpMessageBenchmarks.Encode_AmqpNetLite",
            "value": 703.745,
            "range": "± 4.832",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080622\nmean 703.961 ns"
          },
          {
            "name": "AmqpMessageBenchmarks.Encode",
            "value": 901.659,
            "range": "± 11.935",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080622\nmean 903.458 ns"
          },
          {
            "name": "AmqpMessageSectionBenchmarks.Decode(Sections=Annotations)",
            "value": 832.152,
            "range": "± 9.097",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080622\nmean 831.317 ns"
          },
          {
            "name": "AmqpMessageSectionBenchmarks.Decode_AmqpNetLite(Sections=Annotations)",
            "value": 628.231,
            "range": "± 13.988",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080622\nmean 629.065 ns"
          },
          {
            "name": "AmqpMessageSectionBenchmarks.Decode(Sections=Sequence)",
            "value": 153.256,
            "range": "± 3.794",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080622\nmean 153.642 ns"
          },
          {
            "name": "AmqpMessageSectionBenchmarks.Decode_AmqpNetLite(Sections=Sequence)",
            "value": 142.826,
            "range": "± 1.789",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080622\nmean 142.742 ns"
          },
          {
            "name": "AmqpMessageSectionBenchmarks.Encode(Sections=Annotations)",
            "value": 632.953,
            "range": "± 4.986",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080622\nmean 632.692 ns"
          },
          {
            "name": "AmqpMessageSectionBenchmarks.Encode_AmqpNetLite(Sections=Annotations)",
            "value": 673.875,
            "range": "± 1.342",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080622\nmean 673.874 ns"
          },
          {
            "name": "AmqpMessageSectionBenchmarks.Encode(Sections=Sequence)",
            "value": 268.602,
            "range": "± 1.982",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080622\nmean 268.874 ns"
          },
          {
            "name": "AmqpMessageSectionBenchmarks.Encode_AmqpNetLite(Sections=Sequence)",
            "value": 198.483,
            "range": "± 1.003",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080622\nmean 198.441 ns"
          },
          {
            "name": "AmqpTransferPayloadBenchmarks.Encode(PayloadSize=64)",
            "value": 374.802,
            "range": "± 2.502",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080622\nmean 374.609 ns"
          },
          {
            "name": "AmqpTransferPayloadBenchmarks.Decode(PayloadSize=64)",
            "value": 165.564,
            "range": "± 1.57",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080622\nmean 165.506 ns"
          },
          {
            "name": "AmqpTransferPayloadBenchmarks.Encode(PayloadSize=16384)",
            "value": 3483.751,
            "range": "± 42.005",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080622\nmean 3490.421 ns"
          },
          {
            "name": "AmqpTransferPayloadBenchmarks.Decode(PayloadSize=16384)",
            "value": 1186.772,
            "range": "± 19.391",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080622\nmean 1186.668 ns"
          },
          {
            "name": "AmqpValueBenchmarks.Decode(Shape=Nested)",
            "value": 937.389,
            "range": "± 7.8",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080622\nmean 937.38 ns"
          },
          {
            "name": "AmqpValueBenchmarks.Decode_AmqpNetLite(Shape=Nested)",
            "value": 1131.558,
            "range": "± 10.02",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080622\nmean 1133.573 ns"
          },
          {
            "name": "AmqpValueBenchmarks.Decode(Shape=Numeric)",
            "value": 254.267,
            "range": "± 2.413",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080622\nmean 254.075 ns"
          },
          {
            "name": "AmqpValueBenchmarks.Decode_AmqpNetLite(Shape=Numeric)",
            "value": 298.764,
            "range": "± 2.19",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080622\nmean 299.202 ns"
          },
          {
            "name": "AmqpValueBenchmarks.Decode(Shape=TextBinary)",
            "value": 312.769,
            "range": "± 3.537",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080622\nmean 313.469 ns"
          },
          {
            "name": "AmqpValueBenchmarks.Decode_AmqpNetLite(Shape=TextBinary)",
            "value": 370.688,
            "range": "± 13.419",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080622\nmean 372.914 ns"
          },
          {
            "name": "AmqpValueBenchmarks.Encode(Shape=Nested)",
            "value": 1966.456,
            "range": "± 52.617",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080622\nmean 1971.626 ns"
          },
          {
            "name": "AmqpValueBenchmarks.Encode_AmqpNetLite(Shape=Nested)",
            "value": 1520.216,
            "range": "± 8.77",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080622\nmean 1519.78 ns"
          },
          {
            "name": "AmqpValueBenchmarks.Encode(Shape=Numeric)",
            "value": 335.487,
            "range": "± 1.599",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080622\nmean 335.651 ns"
          },
          {
            "name": "AmqpValueBenchmarks.Encode_AmqpNetLite(Shape=Numeric)",
            "value": 467.965,
            "range": "± 1.779",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080622\nmean 468.042 ns"
          },
          {
            "name": "AmqpValueBenchmarks.Encode(Shape=TextBinary)",
            "value": 784.677,
            "range": "± 11.583",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080622\nmean 783.658 ns"
          },
          {
            "name": "AmqpValueBenchmarks.Encode_AmqpNetLite(Shape=TextBinary)",
            "value": 469.457,
            "range": "± 5.829",
            "unit": "ns",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080622\nmean 469.235 ns"
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
          "id": "699980c0cf28b76854e64dc51e4e45d2c5c05dee",
          "message": "Merge pull request #8 from assimalign/feature/benchmark-suite-expansion\n\nBuild out the benchmark suites: 148 to 988 cases across every area but Database",
          "timestamp": "2026-10-20T07:59:35Z",
          "url": "https://github.com/assimalign/cohesion-benchmarks/commit/699980c0cf28b76854e64dc51e4e45d2c5c05dee"
        },
        "date": 1792485032274,
        "tool": "customSmallerIsBetter",
        "benches": [
          {
            "name": "AmqpDataMessageBenchmarks.Decode(PayloadSize=64) - allocated",
            "value": 496,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080622"
          },
          {
            "name": "AmqpDataMessageBenchmarks.Decode_AmqpNetLite(PayloadSize=64) - allocated",
            "value": 312,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080622"
          },
          {
            "name": "AmqpDataMessageBenchmarks.Decode(PayloadSize=16384) - allocated",
            "value": 16816,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080622"
          },
          {
            "name": "AmqpDataMessageBenchmarks.Decode_AmqpNetLite(PayloadSize=16384) - allocated",
            "value": 16632,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080622"
          },
          {
            "name": "AmqpDataMessageBenchmarks.Encode(PayloadSize=64) - allocated",
            "value": 464,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080622"
          },
          {
            "name": "AmqpDataMessageBenchmarks.Encode_AmqpNetLite(PayloadSize=64) - allocated",
            "value": 200,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080622"
          },
          {
            "name": "AmqpDataMessageBenchmarks.Encode(PayloadSize=16384) - allocated",
            "value": 33448,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080622"
          },
          {
            "name": "AmqpDataMessageBenchmarks.Encode_AmqpNetLite(PayloadSize=16384) - allocated",
            "value": 16736,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080622"
          },
          {
            "name": "AmqpDeliveryFrameBenchmarks.Encode(Kind=Credit) - allocated",
            "value": 1200,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080622"
          },
          {
            "name": "AmqpDeliveryFrameBenchmarks.Decode(Kind=Credit) - allocated",
            "value": 432,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080622"
          },
          {
            "name": "AmqpDeliveryFrameBenchmarks.Encode(Kind=Disposition) - allocated",
            "value": 1184,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080622"
          },
          {
            "name": "AmqpDeliveryFrameBenchmarks.Decode(Kind=Disposition) - allocated",
            "value": 392,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080622"
          },
          {
            "name": "AmqpDeliveryFrameBenchmarks.Encode(Kind=Drain) - allocated",
            "value": 1720,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080622"
          },
          {
            "name": "AmqpDeliveryFrameBenchmarks.Decode(Kind=Drain) - allocated",
            "value": 1104,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080622"
          },
          {
            "name": "AmqpFrameBenchmarks.EncodeTransferFrame - allocated",
            "value": 3352,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080622"
          },
          {
            "name": "AmqpFrameBenchmarks.DecodeTransferFrame - allocated",
            "value": 888,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080622"
          },
          {
            "name": "AmqpMessageBenchmarks.Decode_AmqpNetLite - allocated",
            "value": 1464,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080622"
          },
          {
            "name": "AmqpMessageBenchmarks.Decode - allocated",
            "value": 1856,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080622"
          },
          {
            "name": "AmqpMessageBenchmarks.Encode_AmqpNetLite - allocated",
            "value": 1056,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080622"
          },
          {
            "name": "AmqpMessageBenchmarks.Encode - allocated",
            "value": 2872,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080622"
          },
          {
            "name": "AmqpMessageSectionBenchmarks.Decode(Sections=Annotations) - allocated",
            "value": 2120,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080622"
          },
          {
            "name": "AmqpMessageSectionBenchmarks.Decode_AmqpNetLite(Sections=Annotations) - allocated",
            "value": 1632,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080622"
          },
          {
            "name": "AmqpMessageSectionBenchmarks.Decode(Sections=Sequence) - allocated",
            "value": 584,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080622"
          },
          {
            "name": "AmqpMessageSectionBenchmarks.Decode_AmqpNetLite(Sections=Sequence) - allocated",
            "value": 408,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080622"
          },
          {
            "name": "AmqpMessageSectionBenchmarks.Encode(Sections=Annotations) - allocated",
            "value": 1808,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080622"
          },
          {
            "name": "AmqpMessageSectionBenchmarks.Encode_AmqpNetLite(Sections=Annotations) - allocated",
            "value": 368,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080622"
          },
          {
            "name": "AmqpMessageSectionBenchmarks.Encode(Sections=Sequence) - allocated",
            "value": 776,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080622"
          },
          {
            "name": "AmqpMessageSectionBenchmarks.Encode_AmqpNetLite(Sections=Sequence) - allocated",
            "value": 200,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080622"
          },
          {
            "name": "AmqpTransferPayloadBenchmarks.Encode(PayloadSize=64) - allocated",
            "value": 1320,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080622"
          },
          {
            "name": "AmqpTransferPayloadBenchmarks.Decode(PayloadSize=64) - allocated",
            "value": 472,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080622"
          },
          {
            "name": "AmqpTransferPayloadBenchmarks.Encode(PayloadSize=16384) - allocated",
            "value": 50992,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080622"
          },
          {
            "name": "AmqpTransferPayloadBenchmarks.Decode(PayloadSize=16384) - allocated",
            "value": 16792,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080622"
          },
          {
            "name": "AmqpValueBenchmarks.Decode(Shape=Nested) - allocated",
            "value": 3352,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080622"
          },
          {
            "name": "AmqpValueBenchmarks.Decode_AmqpNetLite(Shape=Nested) - allocated",
            "value": 3392,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080622"
          },
          {
            "name": "AmqpValueBenchmarks.Decode(Shape=Numeric) - allocated",
            "value": 824,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080622"
          },
          {
            "name": "AmqpValueBenchmarks.Decode_AmqpNetLite(Shape=Numeric) - allocated",
            "value": 800,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080622"
          },
          {
            "name": "AmqpValueBenchmarks.Decode(Shape=TextBinary) - allocated",
            "value": 2048,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080622"
          },
          {
            "name": "AmqpValueBenchmarks.Decode_AmqpNetLite(Shape=TextBinary) - allocated",
            "value": 2024,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080622"
          },
          {
            "name": "AmqpValueBenchmarks.Encode(Shape=Nested) - allocated",
            "value": 10648,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080622"
          },
          {
            "name": "AmqpValueBenchmarks.Encode_AmqpNetLite(Shape=Nested) - allocated",
            "value": 2832,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080622"
          },
          {
            "name": "AmqpValueBenchmarks.Encode(Shape=Numeric) - allocated",
            "value": 744,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080622"
          },
          {
            "name": "AmqpValueBenchmarks.Encode_AmqpNetLite(Shape=Numeric) - allocated",
            "value": 200,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080622"
          },
          {
            "name": "AmqpValueBenchmarks.Encode(Shape=TextBinary) - allocated",
            "value": 6056,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080622"
          },
          {
            "name": "AmqpValueBenchmarks.Encode_AmqpNetLite(Shape=TextBinary) - allocated",
            "value": 2168,
            "unit": "B",
            "extra": "cohesion 10.0.0-preview.0.ci.47 (321a43d7e084) on GitHub Actions 1000080622"
          }
        ]
      }
    ]
  }
}