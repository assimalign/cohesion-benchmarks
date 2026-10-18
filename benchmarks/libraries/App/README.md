# App

No benchmark suite. cohesion's `libraries/App` contains no runtime code: its projects
(`Assimalign.Cohesion.App.Refs` / `.Runtime`) only pack the `Assimalign.Cohesion.App` shared framework from
members built elsewhere, and those members (Hosting, Configuration, Connections, ...) are measured in their own
areas. A suite belongs here only if App gains code of its own, for example framework resolution at application
startup, which would be measured as process start time of a minimal `Sdk.<Area>` application.
