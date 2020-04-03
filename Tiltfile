# TODO: use different dockerfiles based on env? Is it possible with tilt?

k8s_yaml(['webapp/deployment.yaml'])
k8s_yaml(['webapi/src/GraphQLApi/deployment.yaml'])
docker_build('webapp', './webapp/', 
    live_update=[
        # when package.json changes, we need to do a full build
        fall_back_on(['package.json', 'package-lock.json']),
        sync('.', '/'),
    ]
)
docker_build('webapi', './webapi/src/GraphQLApi/',
    live_update=[
        sync('.', '/'),
    ]
)