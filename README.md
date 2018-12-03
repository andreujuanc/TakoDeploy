# TakoDeploy
Best multi database sql script deployment tool

[![Join the chat at https://gitter.im/andreujuanc/TakoDeploy](https://badges.gitter.im/andreujuanc/TakoDeploy.svg)](https://gitter.im/andreujuanc/TakoDeploy?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

[![Build Status](https://dev.azure.com/andreujuan/TakoDeploy/_apis/build/status/TakoDeploy-.NET%20Desktop-CI)](https://dev.azure.com/andreujuan/TakoDeploy/_build/latest?definitionId=14)

### Read the Docs
https://andreujuanc.github.io/TakoDeploy/

### Features:
 - Get all databases from a single instance and apply a name filter. Or just a single direct connection.
 - Mix database sources as much as you want. Example, two direct and one full instance with or withut a filter.
 - Script editor (Avalon Text, same monodevelop uses)
 - Scripts are parsed and errors are detected before executing.
 - Scripts are 'splitted' by GO statements.
 - Save your deployment into a file
 - Get a list of all databases before deploying.
 - See in realtime what is happening (PRINT statements are recommended here!).
 - List of messages and errors for each database during and after validation/deployment.
 - Link errors to files (beta).
 - Automatic rollback to independent database if any error occurs.
 - Transparent Updates via Squirrel.
 - Cancel validation/deployment.

## Restrictions
 - For now only Sql Server is tested and supported
 - Your scripts cannot contain USE [database] statements
 
## Special Thanks
 - We are using [Material Design In XAML Toolkit](https://github.com/ButchersBoy/MaterialDesignInXamlToolkit). The best WPF toolkit out there!

## TODO:
There is some stuff to do that will improve the project in a big way. If you are interested to see what's comming or if you want to help please check the project's board:
 - https://github.com/andreujuanc/TakoDeploy/projects/1#column-3537715

## Download the latest version
https://github.com/andreujuanc/TakoDeploy/releases/latest
