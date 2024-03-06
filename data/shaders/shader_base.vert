#version 330

layout (location = 0) in vec3 aPos;
layout (location = 1) in vec3 aNorm;
out vec3 Normals;
out vec3 FragPos; 
uniform float angle;

uniform mat4 projection;
uniform mat4 view;
uniform mat4 model;
uniform mat4 local;

void main()
{
    vec3 axis = vec3(0.0, 1.0, 0.0);
    float angl = angle;
    gl_Position =  projection* view * model * local  * vec4(aPos, 1.0);
    Normals = aNorm;
    FragPos = vec3(model * vec4(aPos, 1.0f));
}
