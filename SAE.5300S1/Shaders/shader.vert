#version 330 core
layout (location = 0) in vec3 vPos;
layout (location = 2) in vec2 vTexCoords;

uniform mat4 uModel;
uniform mat4 uView;
uniform mat4 uProjection;


out vec2 fTexCoords;

void main()
{
    //Multiplying our uniform with the vertex position, the multiplication order here does matter.
    gl_Position = uProjection * uView * uModel * vec4(vPos, 1.0);
    //Pass the texture coordinates straight through to the fragment shader
    fTexCoords = vTexCoords;
}