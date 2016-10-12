# TakoDeploy
Best sql script deploy tool

[![Join the chat at https://gitter.im/andreujuanc/TakoDeploy](https://badges.gitter.im/andreujuanc/TakoDeploy.svg)](https://gitter.im/andreujuanc/TakoDeploy?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

![VSTSBuild](https://andreujuan.visualstudio.com/_apis/public/build/definitions/78c4047a-c300-49e5-aaa6-dfa1325a3dcb/1/badge)

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
 - Automatic rollback to independent database if any error occurs.
 - Transparent Updates via Squirrel.

## Restrictions
 - For now only Sql Server is tested
 - Your script cannot contain USE [database] statements
 
## TODO:
 - Code more tests
 - Implement more providers
 - Options and settings?
 - Save a document as Source Document, so no scripts are saved in it.
 - Add more info after deployment is done.
 - Theme selector? C:

## Download the latest version
https://github.com/andreujuanc/TakoDeploy/releases/latest
