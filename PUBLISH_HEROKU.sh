ECHO login Heroku...
heroku container:login

ECHO build na imagem...
docker build --rm -f "Dockerfile" -t "nid-todo-api:latest" .

ECHO preparando imagem para envio...
heroku container:push web -a nid-todo-api

ECHO subindo imagem
heroku container:release web -a nid-todo-api