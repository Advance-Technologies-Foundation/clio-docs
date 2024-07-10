docker stop test_bpm
docker rm test_bpm
docker build -f Dockerfile -t bpm .
docker run -p 99:5000 -p 98:5002 -d --dns=10.0.7.1 --dns=10.0.7.2 --dns-search=tscrm.com --name test_bpm bpm