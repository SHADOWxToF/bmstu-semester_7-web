```
ab.exe -c 100 -n 10000 http://localhost/api/v1
```

```
This is ApacheBench, Version 2.3 <$Revision: 1903618 $>
Copyright 1996 Adam Twiss, Zeus Technology Ltd, http://www.zeustech.net/
Licensed to The Apache Software Foundation, http://www.apache.org/

Benchmarking localhost (be patient)
Completed 1000 requests
Completed 2000 requests
Completed 3000 requests
Completed 4000 requests
Completed 5000 requests
Completed 6000 requests
Completed 7000 requests
Completed 8000 requests
Completed 9000 requests
Completed 10000 requests
Finished 10000 requests


Server Software:        nginx
Server Hostname:        localhost
Server Port:            80

Document Path:          /api/v1
Document Length:        162 bytes

Concurrency Level:      100
Time taken for tests:   13.571 seconds
Complete requests:      10000
Failed requests:        0
Non-2xx responses:      10000
Total transferred:      3710000 bytes
HTML transferred:       1620000 bytes
Requests per second:    736.86 [#/sec] (mean)
Time per request:       135.710 [ms] (mean)
Time per request:       1.357 [ms] (mean, across all concurrent requests)
Transfer rate:          266.97 [Kbytes/sec] received

Connection Times (ms)
              min  mean[+/-sd] median   max
Connect:        0    0   0.3      0       4
Processing:    17  135  42.9    130     276
Waiting:        4  132  42.9    128     275
Total:         17  135  42.9    130     276

Percentage of the requests served within a certain time (ms)
  50%    130
  66%    152
  75%    164
  80%    172
  90%    192
  95%    212
  98%    231
  99%    248
 100%    276 (longest request)
```




```
ab.exe -c 100 -n 10000 http://localhost/api/v1
```

```
This is ApacheBench, Version 2.3 <$Revision: 1903618 $>
Copyright 1996 Adam Twiss, Zeus Technology Ltd, http://www.zeustech.net/
Licensed to The Apache Software Foundation, http://www.apache.org/

Benchmarking localhost (be patient)
Completed 1000 requests
Completed 2000 requests
Completed 3000 requests
Completed 4000 requests
Completed 5000 requests
Completed 6000 requests
Completed 7000 requests
Completed 8000 requests
Completed 9000 requests
Completed 10000 requests
Finished 10000 requests


Server Software:        nginx
Server Hostname:        localhost
Server Port:            80

Document Path:          /api/v1
Document Length:        162 bytes

Concurrency Level:      100
Time taken for tests:   12.215 seconds
Complete requests:      10000
Failed requests:        0
Non-2xx responses:      10000
Total transferred:      3710000 bytes
HTML transferred:       1620000 bytes
Requests per second:    818.64 [#/sec] (mean)
Time per request:       122.154 [ms] (mean)
Time per request:       1.222 [ms] (mean, across all concurrent requests)
Transfer rate:          296.60 [Kbytes/sec] received

Connection Times (ms)
              min  mean[+/-sd] median   max
Connect:        0    0   0.3      0       3
Processing:    18  121  41.7    116     286
Waiting:        2  119  41.8    113     282
Total:         19  122  41.7    116     286

Percentage of the requests served within a certain time (ms)
  50%    116
  66%    132
  75%    143
  80%    153
  90%    178
  95%    200
  98%    231
  99%    248
 100%    286 (longest request)
 ```