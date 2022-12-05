rem Reset WINSOCK entries to installation defaults: 
netsh winsock reset catalog
rem Reset TCP/IP stack to installation defaults: 
netsh int ip reset reset.log
rem Clear ARP Cache: 
netsh interface ip delete arpcache
rem Flush DNS cache: 
ipconfig /flushdns
rem  Flush routing table (reboot required): 
rem route /f