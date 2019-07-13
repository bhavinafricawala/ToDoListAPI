docker build .

heroku container:login

heroku container:push web -a ba-todolistapi

heroku container:release web -a ba-todolistapi