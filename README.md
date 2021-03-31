# Streaming API Prototype
This is an experiment in using SignalR to create a streaming API. 

A Streaming API allows a system to connect to an endpoint and receive updates, allowing it to catch up if the system went offline by mistake.
This reduces the huge amount of load when servers connect to Restful endpoints and try to pull down all changed data via polling. 
This prototype was designed for Tesco Counters that need constant product updates and to cater for bad connnections or faults.
