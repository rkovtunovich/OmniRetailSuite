# base image
FROM alpine:latest

# set consul version
ENV CONSUL_VERSION 1.18.0

# create a new directory
RUN mkdir /consul

# download dependencies
RUN apk --no-cache add \
      bash \
      ca-certificates \
      wget \
      curl

# download and set up consul
RUN wget --quiet --output-document=/tmp/consul.zip https://releases.hashicorp.com/consul/${CONSUL_VERSION}/consul_${CONSUL_VERSION}_linux_amd64.zip && \
    unzip /tmp/consul.zip -d /consul && \
    rm -f /tmp/consul.zip && \
    chmod +x /consul/consul

# update PATH
ENV PATH="PATH=$PATH:$PWD/consul"

# add the config file
COPY consul-config.json /consul/config/config.json

# expose ports
EXPOSE 8300 8400 8500 8600

# run consul
ENTRYPOINT ["consul"]